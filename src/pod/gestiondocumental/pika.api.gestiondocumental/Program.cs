
using apigenerica.primitivas;
using apigenerica.primitivas.aplicacion;
using apigenerica.primitivas.seguridad;
using comunes.interservicio.primitivas;
using comunes.interservicio.primitivas.seguridad;
using Microsoft.EntityFrameworkCore;
using pika.api.gestiondocumental.seguridad;
using pika.servicios.gestiondocumental;
using System.Reflection;

public class Program
{
    /// <summary>
    /// Realiza las migraciones en las bases de datos
    /// </summary>
    /// <param name="app"></param>
    private static void UpdateDatabase(IApplicationBuilder app)
    {
        using (var serviceScope = app.ApplicationServices
            .GetRequiredService<IServiceScopeFactory>()
            .CreateScope())
        {
            using (var context = serviceScope.ServiceProvider.GetService<DbContextGestionDocumental>())
            {
                context!.Database.Migrate();
            }
        }
    }

    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        var configuration = builder.Configuration;
        builder.CreaConfiguracionStandar(Assembly.GetExecutingAssembly());
        builder.Services.Configure<ConfiguracionAPI>(builder.Configuration.GetSection(nameof(ConfiguracionAPI)));
        builder.CreaConfiguiracionEntidadGenerica();

        var connectionString = builder.Configuration.GetConnectionString("pika-gestiondocumental");

        builder.Services.AddDbContext<DbContextGestionDocumental>(options =>
        {
            options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
        });

        builder.Services.AddControllers();

        builder.Services.AddCors(c =>
        {
            c.AddPolicy("default", p =>
            {
                p.AllowAnyMethod();
                p.AllowAnyOrigin();
                p.AllowAnyHeader();
            });
        });

        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        builder.Services.AddSingleton<IProveedorAplicaciones, ConfiguracionSeguridad>();
        builder.Services.AddSingleton<ICacheSeguridad, CacheSeguridad>();
        builder.Services.AddTransient<IProxySeguridad, ProxySeguridad>();
        builder.Services.AddTransient<ICacheAtributos, CacheAtributos>();
        builder.Services.AddDistributedMemoryCache();
        // Añadir la extensión para los servicios de API genérica
        builder.Services.AddServiciosEntidadAPI();
        builder.Services.AddAuthentication("Bearer")
        .AddJwtBearer("Bearer", options =>
        {
            options.Authority = "https://localhost:7001";
            options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
            {
                ValidateAudience = false,
                ValidateIssuer = false
            };
        });
        builder.Services.AddAuthorization(options =>
        {
            options.AddPolicy("InternalScope", policy =>
            {
                policy.AuthenticationSchemes = new[] { "Bearer" };
                policy.RequireAuthenticatedUser();
                policy.RequireClaim("scope", "internal");
            });
        });
        var app = builder.Build();
        UpdateDatabase(app);

        app.UseEntidadAPI();
        app.UseCors("default");
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }
        app.UseHttpsRedirection();
        app.UseAuthentication();
        app.UseAuthorization();
        app.MapControllers();
        app.Run();
    }
}
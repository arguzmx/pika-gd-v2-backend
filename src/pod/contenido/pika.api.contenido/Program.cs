using apigenerica.primitivas;
using apigenerica.primitivas.aplicacion;
using apigenerica.primitivas.seguridad;
using comunes.interservicio.primitivas;
using comunes.interservicio.primitivas.seguridad;
using Microsoft.EntityFrameworkCore;
using pika.api.contenido.seguridad;
using pika.servicios.contenido.dbcontext;
using System.Reflection;

namespace pika.api.contenido;
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
            using (var context = serviceScope.ServiceProvider.GetService<DbContextContenido>())
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

        var connectionString = builder.Configuration.GetConnectionString("pika-contenido");

        builder.Services.AddDbContext<DbContextContenido>(options =>
        {
            options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
        });

        // Add services to the container.
        builder.Services.AddCors(c =>
        {
            c.AddPolicy("default", p =>
            {
                p.AllowAnyMethod();
                p.AllowAnyOrigin();
                p.AllowAnyHeader();
            });
        });


        //builder.Services.AddCouchContext<ContenidoCouchDbContext>(builder => builder
        //    .EnsureDatabaseExists()
        //    .UseEndpoint(configuration.GetValue<string>("CouchDB:endpoint")!)
        //    .UseBasicAuthentication(username: configuration.GetValue<string>("CouchDB:username")!,
        //    password: configuration.GetValue<string>("CouchDB:password")!));


        builder.Services.AddControllers();

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

#if DEBUG
        app.DummyData().Wait();
#endif
        app.CatalogoGenericos().Wait();
        app.UseEntidadAPI();

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }
        
        app.UseHttpsRedirection();
        app.UseAuthentication();
        app.UseAuthorization();
        app.MapControllers();
        app.UseCors("default");
        app.Run();
    }
}
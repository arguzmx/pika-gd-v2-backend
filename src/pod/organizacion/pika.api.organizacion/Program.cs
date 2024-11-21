using apigenerica.model.reflectores;
using apigenerica.primitivas;
using apigenerica.primitivas.aplicacion;
using apigenerica.primitivas.seguridad;
using comunes.interservicio.primitivas;
using comunes.interservicio.primitivas.seguridad;
using Microsoft.EntityFrameworkCore;
using pika.api.organizacion.seguridad;
using pika.servicios.organizacion.dbcontext;
using Quartz;
using Serilog;
using System.Reflection;

namespace pika.api.organizacion;

public class Program
{
    private static void UpdateDatabase(IApplicationBuilder app)
    {
        using (var serviceScope = app.ApplicationServices
            .GetRequiredService<IServiceScopeFactory>()
            .CreateScope())
        {
            using (var context = serviceScope.ServiceProvider.GetService<DbContextOrganizacion>())
            {
                context.Database.Migrate();
            }
        }
    }

    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        IWebHostEnvironment environment = builder.Environment;


        builder.Services.AddQuartz(options =>
        {
            options.UseMicrosoftDependencyInjectionJobFactory();
            options.UseSimpleTypeLoader();
            options.UseInMemoryStore();
        });

        // Register the Quartz.NET service and configure it to block shutdown until jobs are complete.
        builder.Services.AddQuartzHostedService(options => options.AwaitApplicationStarted = true);

        builder.CreaConfiguracionStandar(Assembly.GetExecutingAssembly());
        builder.Services.Configure<ConfiguracionAPI>(builder.Configuration.GetSection(nameof(ConfiguracionAPI)));
        builder.CreaConfiguiracionEntidadGenerica();

        // Add services to the container.  observar en el GetConnectionString Quiza sea pika-organizacion
        var connectionString = builder.Configuration.GetConnectionString("pika-organizacion");

        builder.Services.AddDbContext<DbContextOrganizacion>(options =>
        {
            options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
        });

        builder.Services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();



        builder.Services.AddSingleton<IProveedorAplicaciones, ConfiguracionSeguridad>();
        builder.Services.AddSingleton<ICacheSeguridad, CacheSeguridad>();
        builder.Services.AddSingleton<IProxySeguridad, ProxySeguridad>();
        builder.Services.AddTransient<ICacheAtributos, CacheAtributos>();
        builder.Services.AddHttpClient();



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

        builder.Services.AddHostedService<Worker>();
        var app = builder.Build();

        UpdateDatabase(app);

        // A�adir la extensi�n para los servicios de API gen�rica
        app.UseEntidadAPI();

        // Configure the HTTP request pipeline.
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
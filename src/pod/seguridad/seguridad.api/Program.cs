using apigenerica.primitivas;
using apigenerica.primitivas.aplicacion;
using apigenerica.primitivas.seguridad;
using comunes.interservicio.primitivas;
using comunes.interservicio.primitivas.seguridad;
using comunes.primitivas.configuracion.mongo;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using seguridad.api.helpers;
using seguridad.api.seguridad;
using seguridad.modelo.servicios;
using seguridad.modelo.servicios.mysql;
using seguridad.servicios;
using seguridad.servicios.mysql;
using System.Reflection;
namespace seguridad.api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            IWebHostEnvironment environment = builder.Environment;
            builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{environment.EnvironmentName}.json", optional: true, reloadOnChange: true)
                .AddUserSecrets(Assembly.GetExecutingAssembly(), true)
                .AddEnvironmentVariables();

            var configuration = builder.Configuration;

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

            builder.Services.AddControllers();

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.Configure<ConfiguracionAPI>(builder.Configuration.GetSection(nameof(ConfiguracionAPI)));
            builder.Services.AddSingleton<IConfigureOptions<ConfiguracionMongo>, ConfigureConfiguracionMongoOptions>();
            builder.Services.AddSingleton<IServicionConfiguracionMongo, ServicioConfiguracionMongoOptions>();
            builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            string driver = builder.Configuration.GetValue<string>("driver")!;

            switch (driver.ToLower())
            {

                case "mongo":
                    builder.Services.AddTransient<IServicioAplicacion, ServicioAplicacion>();
                    builder.Services.AddSingleton<IServicioInstanciaAplicacion, ServicioInstanciaAplicacion>();
                    break;

                case "mysql":
                    var connectionStringMySql = builder.Configuration.GetConnectionString("pika-seguridad");
                    builder.Services.AddDbContext<DBContextMySql>(options =>
                    {
                        options.UseMySql(connectionStringMySql, ServerVersion.AutoDetect(connectionStringMySql));

                    });
                    builder.Services.AddTransient<IServicioAplicacion, ServicioAplicacionMysql>();
                    builder.Services.AddTransient<IServicioInstanciaAplicacion, ServicioInstanciaAplicacionMySql>();
                    break;

                default:
                    throw new Exception($"Driver no v�lido {driver}");
            }
            builder.Services.AddTransient<IProveedorAplicaciones, ConfiguracionSeguridad>();
            builder.Services.AddSingleton<ICacheSeguridad, CacheSeguridad>();
            builder.Services.AddTransient<IProxySeguridad, ProxySeguridad>();
            builder.Services.AddTransient<IServicioAutenticacionJWT, ServicioAuthInterprocesoJWT>();
            builder.Services.AddTransient<ICacheAtributos, CacheAtributos>();
            builder.Services.AddHttpClient();
            builder.CreaConfiguiracionEntidadGenerica();

            var app = builder.Build();
            switch (driver.ToLower())
            {

                case "mysql":
                    app.DBContextMySqlUpdateDatabase();
                    break;

                default:
                    break;
            }
            // A?adir la extensi?n para los servicios de API gen?rica
            app.UseEntidadAPI();
            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            app.UseCors("default");
            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}

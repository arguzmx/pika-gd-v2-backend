using apigenerica.model.modelos;
using Microsoft.EntityFrameworkCore;
using pika.modelo.organizacion;
using pika.modelo.organizacion.Contacto;
using pika.servicios.organizacion.dbcontext.configuraciones;

namespace pika.servicios.organizacion.dbcontext;

public class DbContextOrganizacion : DbContext
{
    public const string TABLA_DOMINIO = "org$dominio";
    public const string TABLA_UNIDADES_ORG = "org$unidadorganizacional";
    public const string TABLA_USUARIO_DOMINIO = "org$usuariodominio";
    public const string TABLA_USUARIO_UNIDADES_ORG = "org$usuariounidadorganizacional";
    public const string TABLA_PUESTO = "org$puesto";
    public const string TABLA_DIRECCIONPOSTAL = "org$direccionpostal";
    public const string TABLA_TELEFONO = "org$telefono";
    public const string TABLA_REDSOCIAL = "org$redsocial";

    public DbContextOrganizacion(DbContextOptions<DbContextOrganizacion> options) : base(options) 
    {
        
    }

    public DbSet<Dominio> Dominios { get; set; }
    public DbSet<UnidadOrganizacional> UnidadesOrganizacionales { get; set; }
    public DbSet<UsuarioDominio> UsuarioDominios { get; set; }
    public DbSet<UsuarioUnidadOrganizacional> UsuariosUnidadesOrganizacionales { get; set; }
    public DbSet<Puesto> Puestos { get; set; }
    public DbSet<DireccionPostal> DireccionesPostales { get; set; }
    public DbSet<Telefono> Telefonos { get; set; }
    public DbSet<RedSocial> RedesSociales { get; set; }
    public DbSet<ElementoCatalogo> TipoRedSocial { get; set; }
    public DbSet<I18NCatalogo> TraduccionesTipoRedSocial { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new ConfiguracionDominio());
        modelBuilder.ApplyConfiguration(new ConfiguracionUnidadOrganizacional());
        modelBuilder.ApplyConfiguration(new ConfiguracionUsuarioDominio());
        modelBuilder.ApplyConfiguration(new ConfiguracionUsuarioUnidadOrganizacional());
        modelBuilder.ApplyConfiguration(new ConfiguracionPuesto());
        modelBuilder.ApplyConfiguration(new ConfiguracionDireccionPostal());
        modelBuilder.ApplyConfiguration(new ConfiguracionTelefono());
        modelBuilder.ApplyConfiguration(new ConfiguracionRedSocial());
        modelBuilder.ApplyConfiguration(new ConfiguracionI18NCatalogo());
        modelBuilder.ApplyConfiguration(new ConfiguracionElementoCatalogo());

        base.OnModelCreating(modelBuilder);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
    }

}

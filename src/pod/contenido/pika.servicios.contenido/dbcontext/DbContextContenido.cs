using apigenerica.model.modelos;
using Microsoft.EntityFrameworkCore;
using pika.modelo.contenido;
using pika.servicios.contenido.dbcontext.configuraciones;

namespace pika.servicios.contenido.dbcontext;

public class DbContextContenido : DbContext
{
    public const string TablaCarpeta = "cont$carpeta";
    public const string TablaContenido = "cont$contenido";

    public DbContextContenido(DbContextOptions<DbContextContenido> options) : base(options)
    {
    }
    public DbSet<Carpeta> Carpetas { get; set; }
    public DbSet<Contenido> Contenidos { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new ConfiguracionCarpeta());
        modelBuilder.ApplyConfiguration(new ConfiguracionContenido());
        base.OnModelCreating(modelBuilder);
    }


    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
    }
}

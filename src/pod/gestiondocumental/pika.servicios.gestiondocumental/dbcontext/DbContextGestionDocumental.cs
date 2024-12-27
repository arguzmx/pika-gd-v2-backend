using apigenerica.model.modelos;
using Microsoft.EntityFrameworkCore;
using pika.modelo.gestiondocumental;
using pika.servicios.gestiondocumental.dbcontext.configuraciones;
using System;

namespace pika.servicios.gestiondocumental;

public class DbContextGestionDocumental : DbContext
{
    public const string TablaCuadrosClasificacion = "gd$cuadroclasificacion";
    public const string TablaSerieDocumental = "gd$seriedocumental";
    public const string TablaUnidadAdministrativa = "gd$unidadadministrativa";

    public DbContextGestionDocumental(DbContextOptions<DbContextGestionDocumental> options) : base(options)
    {

    }

    public DbSet<SerieDocumental> SerieDocumentales { get; set; }
    public DbSet<CuadroClasificacion> CuadrosClasificacion { get; set; }
    public DbSet<UnidadAdministrativa> UnidadesAdministrativas { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new ConfiguracionCuadroClasificacion());
        modelBuilder.ApplyConfiguration(new ConfiguracionSerieDocumental());
        modelBuilder.ApplyConfiguration(new ConfiguracionUnidadAdministrativa());
        base.OnModelCreating(modelBuilder);
    }


    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
    }
}

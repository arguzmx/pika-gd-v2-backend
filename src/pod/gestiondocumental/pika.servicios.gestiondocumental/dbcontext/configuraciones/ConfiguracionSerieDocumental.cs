

using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using pika.modelo.gestiondocumental;

namespace pika.servicios.gestiondocumental.dbcontext.configuraciones;
public class ConfiguracionSerieDocumental : IEntityTypeConfiguration<SerieDocumental>
{
    public void Configure(EntityTypeBuilder<SerieDocumental> builder)
    {
        builder.ToTable(DbContextGestionDocumental.TablaSerieDocumental);
        builder.HasKey(e => e.Id);

        builder.Property(e => e.Id).IsRequired().HasMaxLength(128);
        builder.Property(e => e.CuadroClasificacionId).IsRequired().HasMaxLength(128);
        builder.Property(e => e.Clave).IsRequired().HasMaxLength(200);
        builder.Property(e => e.Nombre).IsRequired().HasMaxLength(500);
        builder.Property(e => e.Raiz).IsRequired();
        builder.Property(e => e.SeriePadreId).IsRequired(false).HasMaxLength(128);
        builder.Property(e => e.MesesArchivoTramite).IsRequired();
        builder.Property(e => e.MesesArchivoConcentracion).IsRequired();
        builder.Property(e => e.MesesArchivoHistorico).IsRequired();

        builder.HasOne(x => x.CuadroClasificacion).WithMany(y => y.Series).HasForeignKey(z => z.CuadroClasificacionId).OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(x => x.SeriePadre).WithMany(y => y.Subseries).HasForeignKey(z => z.SeriePadreId).OnDelete(DeleteBehavior.Restrict);
    }
}
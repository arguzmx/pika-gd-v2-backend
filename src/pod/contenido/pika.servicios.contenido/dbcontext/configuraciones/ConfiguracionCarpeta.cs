using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using pika.modelo.contenido;
using System.Diagnostics.CodeAnalysis;

namespace pika.servicios.contenido.dbcontext.configuraciones;

[ExcludeFromCodeCoverage]
public class ConfiguracionCarpeta : IEntityTypeConfiguration<Carpeta>
{
    public void Configure(EntityTypeBuilder<Carpeta> builder)
    {
        builder.ToTable(DbContextContenido.TablaCarpeta);
        builder.HasKey(e => e.Id);

        builder.Property(e => e.Id).IsRequired().HasMaxLength(128);
        builder.Property(e => e.RepositorioId).IsConcurrencyToken().HasMaxLength(128);
        builder.Property(e => e.CreadorId).IsRequired().HasMaxLength(128);
        builder.Property(e => e.FechaCreacion).IsRequired();
        builder.Property(e => e.Nombre).IsRequired().HasMaxLength(500);
        builder.Property(e => e.CarpetaPadreId).IsRequired(false).HasMaxLength(128);
        builder.Property(e => e.EsRaiz);
        builder.Property(e => e.PermisoId).HasMaxLength(128);

        builder.HasOne(x => x.Repositorio).WithMany(y => y.Carpetas).HasForeignKey(x => x.RepositorioId).OnDelete(DeleteBehavior.Cascade);
    }
}
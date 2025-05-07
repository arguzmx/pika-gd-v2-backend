using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using pika.modelo.contenido;

namespace pika.servicios.contenido.dbcontext.configuraciones;
public class ConfiguracionContenido : IEntityTypeConfiguration<Contenido>
{
    public void Configure(EntityTypeBuilder<Contenido> builder)
    {
        builder.ToTable(DbContextContenido.TablaContenido);
        builder.HasKey(e => e.Id);

        builder.Property(e => e.Id).IsRequired().HasMaxLength(128);
        builder.Property(e => e.Nombre).IsRequired().HasMaxLength(500);
        builder.Property(e => e.RepositorioId).IsConcurrencyToken().HasMaxLength(128);
        builder.Property(e => e.CreadorId).IsRequired().HasMaxLength(128);
        builder.Property(e => e.FechaCreacion).IsRequired();
        builder.Property(e => e.ConteoAnexos).IsRequired();
        builder.Property(e => e.TamanoBytes).IsRequired();
        builder.Property(e => e.VolumenId).IsRequired().HasMaxLength(128);
        builder.Property(e => e.CarpetaId).IsRequired().HasMaxLength(128);
        builder.Property(e => e.TipoElemento).IsRequired().HasMaxLength(128);
        builder.Property(e => e.IdExterno).IsRequired().HasMaxLength(128);
        builder.Property(e => e.PermisoId).IsRequired(false).HasMaxLength(128);

        //builder.HasOne(x => x.Volumen).WithMany(y => y.Contenido).HasForeignKey(x => x.VolumenId).OnDelete(DeleteBehavior.Cascade);
        builder.HasOne(x => x.Repositorio).WithMany(y => y.Contenido).HasForeignKey(x => x.RepositorioId).OnDelete(DeleteBehavior.Cascade);
        builder.HasOne(x => x.Carpeta).WithMany(y => y.Contenido).HasForeignKey(x => x.CarpetaId).OnDelete(DeleteBehavior.Cascade);

    }
}
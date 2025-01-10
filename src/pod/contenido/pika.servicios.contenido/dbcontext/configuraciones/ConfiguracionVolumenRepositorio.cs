using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using pika.modelo.contenido;

namespace pika.servicios.contenido.dbcontext.configuraciones;

public  class ConfiguracionVolumenRepositorio : IEntityTypeConfiguration<VolumenRepositorio>
{
    public void Configure(EntityTypeBuilder<VolumenRepositorio> builder)
    {
        builder.ToTable(DbContextContenido.TablaVolumenRepositorio);
        builder.HasKey(e => e.Id);

        builder.Property(e => e.Id).IsRequired().HasMaxLength(128);
        builder.Property(e => e.RepositorioId).IsRequired().HasMaxLength(128);
        builder.Property(e => e.VolumenId).IsRequired().HasMaxLength(128);
        builder.Property(e => e.Default).IsRequired();
        builder.Property(e => e.Activo).IsRequired();
        builder.HasIndex(e => new { e.RepositorioId, e.VolumenId});
    }
}

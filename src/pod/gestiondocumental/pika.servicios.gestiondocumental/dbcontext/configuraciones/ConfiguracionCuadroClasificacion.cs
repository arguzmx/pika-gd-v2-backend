using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using pika.modelo.gestiondocumental;

namespace pika.servicios.gestiondocumental.dbcontext.configuraciones;
public class ConfiguracionCuadroClasificacion : IEntityTypeConfiguration<CuadroClasificacion>
{
    public void Configure(EntityTypeBuilder<CuadroClasificacion> builder)
    {
        builder.ToTable(DbContextGestionDocumental.TablaCuadrosClasificacion);
        builder.HasKey(e => e.Id);

        builder.Property(e => e.Id).IsRequired().HasMaxLength(128);
        builder.Property(e => e.Nombre).IsRequired().HasMaxLength(128);
        builder.Property(e => e.DominioId).IsRequired().HasMaxLength(128);
        builder.Property(e => e.UOrgId).IsRequired().HasMaxLength(128);

        //builder.Property(e => e.TipoArchivoId).IsRequired(true).HasMaxLength(128);
    }
}
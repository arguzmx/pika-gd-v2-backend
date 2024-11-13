using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using seguridad.modelo.relaciones;

namespace seguridad.servicios.mysql;

public class ConfiguracionRolGrupo : IEntityTypeConfiguration<RolGrupo>
{
    public void Configure(EntityTypeBuilder<RolGrupo> builder)
    {
        builder.ToTable("seguridad$rolgrupo");
        builder.HasKey(x => x.Id);
        builder.Property(e => e.Id).IsRequired(true);
    }
}

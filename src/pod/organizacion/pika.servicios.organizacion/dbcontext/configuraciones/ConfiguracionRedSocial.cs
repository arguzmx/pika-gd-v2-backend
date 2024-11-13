using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using pika.modelo.organizacion.Contacto;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pika.servicios.organizacion.dbcontext.configuraciones
{
    [ExcludeFromCodeCoverage]
    public class ConfiguracionRedSocial : IEntityTypeConfiguration<RedSocial>
    {

        public void Configure(EntityTypeBuilder<RedSocial> builder)
        {
            builder.ToTable(DbContextOrganizacion.TABLA_REDSOCIAL);
            builder.HasKey(e => e.Id);
            builder.Property(e => e.Id).IsRequired().HasMaxLength(128);
            builder.Property(e => e.Url).IsRequired().HasMaxLength(20);
            builder.Property(e => e.TipoRedSocialId).IsRequired().HasMaxLength(128);
            builder.Property(e => e.DominioId).IsRequired().HasMaxLength(128);
            builder.Property(e => e.UOrgId).IsRequired().HasMaxLength(128);

            builder.HasOne(x => x.TipoRedSocial).WithMany(y => y.RedesSociales).HasForeignKey(z => z.TipoRedSocialId).OnDelete(DeleteBehavior.Cascade);
        }

    }
}

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using pika.modelo.organizacion;
using pika.modelo.organizacion.Contacto;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pika.servicios.organizacion.dbcontext.configuraciones;

[ExcludeFromCodeCoverage]
public class ConfiguracionDireccionPostal : IEntityTypeConfiguration<DireccionPostal>
{

    public void Configure(EntityTypeBuilder<DireccionPostal> builder)
    {
        builder.ToTable(DbContextOrganizacion.TABLA_DIRECCIONPOSTAL);
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id).IsRequired().HasMaxLength(128);
        builder.Property(e => e.Calle).IsRequired().HasMaxLength(500);
        builder.Property(e => e.NoInterior).IsRequired().HasMaxLength(50);
        builder.Property(e => e.NoExterior).IsRequired().HasMaxLength(50);
        builder.Property(e => e.CP).IsRequired().HasMaxLength(20);
        builder.Property(e => e.Pais).IsRequired().HasMaxLength(200);
        builder.Property(e => e.Estado).IsRequired().HasMaxLength(200);
        builder.Property(e => e.Ciudad).IsRequired().HasMaxLength(200);
        builder.Property(e => e.Referencia).IsRequired().HasMaxLength(500);
        builder.Property(e => e.DominioId).IsRequired().HasMaxLength(128);
        builder.Property(e => e.UOrgId).IsRequired().HasMaxLength(128);
    }

}

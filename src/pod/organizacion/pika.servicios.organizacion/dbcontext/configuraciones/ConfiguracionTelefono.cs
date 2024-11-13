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

namespace pika.servicios.organizacion.dbcontext.configuraciones
{
    [ExcludeFromCodeCoverage]
    public class ConfiguracionTelefono : IEntityTypeConfiguration<Telefono>
    {

        public void Configure(EntityTypeBuilder<Telefono> builder)
        {
            builder.ToTable(DbContextOrganizacion.TABLA_TELEFONO);
            builder.HasKey(e => e.Id);
            builder.Property(e => e.Id).IsRequired().HasMaxLength(128);
            builder.Property(e => e.Numero).IsRequired().HasMaxLength(20);
            builder.Property(e => e.Extension).IsRequired().HasMaxLength(200);
            builder.Property(e => e.Horario).IsRequired().HasMaxLength(500);
            builder.Property(e => e.DominioId).IsRequired().HasMaxLength(128);
            builder.Property(e => e.UOrgId).IsRequired().HasMaxLength(128);
        }

    }
}

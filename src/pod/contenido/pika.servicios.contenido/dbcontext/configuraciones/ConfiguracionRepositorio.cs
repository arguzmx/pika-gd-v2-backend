using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using pika.modelo.contenido;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pika.servicios.contenido.dbcontext.configuraciones;

public class ConfiguracionRepositorio : IEntityTypeConfiguration<Repositorio>
{

    public void Configure(EntityTypeBuilder<Repositorio> builder)
    {
        builder.ToTable(DbContextContenido.TablaRepositorio);
        builder.HasKey(e => e.Id);

        builder.Property(e => e.Id).IsRequired().HasMaxLength(128);
        builder.Property(e => e.DominioId).IsRequired().HasMaxLength(128);
        builder.Property(e => e.UOrgId).IsRequired().HasMaxLength(128);
        builder.Property(e => e.Nombre).IsRequired().HasMaxLength(500);
    }

}

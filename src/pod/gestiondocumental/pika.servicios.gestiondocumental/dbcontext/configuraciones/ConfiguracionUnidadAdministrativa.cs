using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using pika.modelo.gestiondocumental;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pika.servicios.gestiondocumental.dbcontext.configuraciones;


public class ConfiguracionUnidadAdministrativa : IEntityTypeConfiguration<UnidadAdministrativa>
{
    public void Configure(EntityTypeBuilder<UnidadAdministrativa> builder)
    {
        builder.ToTable(DbContextGestionDocumental.TablaUnidadAdministrativa);
        builder.HasKey(e => e.Id);

        builder.Property(e => e.Id).IsRequired().HasMaxLength(128);
        builder.Property(e => e.Nombre).IsRequired().HasMaxLength(500);
        builder.Property(e => e.Responsable).IsRequired().HasMaxLength(500);
        builder.Property(e => e.Cargo).IsRequired().HasMaxLength(500);
        builder.Property(e => e.Telefono).IsRequired().HasMaxLength(50);
        builder.Property(e => e.Email).IsRequired().HasMaxLength(50);
        builder.Property(e => e.Domicilio).IsRequired().HasMaxLength(200);
        builder.Property(e => e.UbicacionFisica).IsRequired().HasMaxLength(200);
        builder.Property(e => e.ArchivoTramiteId).IsRequired().HasMaxLength(128);
        builder.Property(e => e.ArchivoConcentracionId).IsRequired().HasMaxLength(128);
        builder.Property(e => e.ArchivoHistoricoId).IsRequired().HasMaxLength(128);

        //builder.HasOne(x => x.ArchivoTramite).WithMany(y => y.UnidadAdministrativasTramite).HasForeignKey(z => z.ArchivoTramiteId).OnDelete(DeleteBehavior.Cascade);
        //builder.HasOne(x => x.ArchivoConcentracion).WithMany(y => y.UnidadAdministrativasConcentracion).HasForeignKey(z => z.ArchivoConcentracionId).OnDelete(DeleteBehavior.Cascade);
        //builder.HasOne(x => x.ArchivoHistorico).WithMany(y => y.UnidadAdministrativasHistorico).HasForeignKey(z => z.ArchivoHistoricoId).OnDelete(DeleteBehavior.Cascade);
    }
}

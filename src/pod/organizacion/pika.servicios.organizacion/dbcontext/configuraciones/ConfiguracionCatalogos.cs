using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using pika.modelo.organizacion.Contacto;
using pika.modelo.organizacion;
using apigenerica.model.modelos;

namespace pika.servicios.organizacion.dbcontext.configuraciones;

/*                       Catalogos              */
public class ConfiguracionElementoCatalogo : IEntityTypeConfiguration<ElementoCatalogo>
{
    public void Configure(EntityTypeBuilder<ElementoCatalogo> builder)
    {
        builder.ToTable("org$catalogos");
        builder.HasDiscriminator<int>("Discriminator")
            .HasValue<ElementoCatalogo>(0)
            .HasValue<TipoRedSocial>(1);


        builder.HasKey(e => new { e.Id });
        builder.Property(e => e.Id).IsRequired().HasMaxLength(128);
        builder.Property(e => e.Texto).IsRequired().HasMaxLength(512);
        builder.Property(e => e.Idioma).IsRequired().HasMaxLength(10);
        builder.Property(e => e.DominioId).IsRequired().HasMaxLength(128);
        builder.Property(e => e.UnidadOrganizacionalId).IsRequired().HasMaxLength(128);
        builder.Property(e => e.CatalogoId).IsRequired().HasMaxLength(128);
        builder.HasIndex(e => e.CatalogoId);
    }
}


/*                   I18Catalogos           */    
public class ConfiguracionI18NCatalogo : IEntityTypeConfiguration<I18NCatalogo>
{
    public void Configure(EntityTypeBuilder<I18NCatalogo> builder)
    {

        builder.ToTable("org$i18ncatalogos");
        builder.HasDiscriminator<int>("Discriminator")
            .HasValue<I18NCatalogo>(0)
            .HasValue<TraduccionesTipoRedSocial>(1);

        builder.HasKey(e => new { e.Id, e.DominioId, e.UnidadOrganizacionalId, e.Idioma });
        builder.Property(e => e.Id).IsRequired().HasMaxLength(128);
        builder.Property(e => e.Texto).IsRequired().HasMaxLength(512);
        builder.Property(e => e.Idioma).IsRequired().HasMaxLength(10);
        builder.Property(e => e.DominioId).IsRequired().HasMaxLength(128);
        builder.Property(e => e.CatalogoId).IsRequired().HasMaxLength(128);
        builder.HasIndex(e => new { e.CatalogoId, e.Idioma });

    }
}

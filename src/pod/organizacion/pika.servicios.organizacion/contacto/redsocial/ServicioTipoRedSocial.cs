using apigenerica.model.modelos;
using apigenerica.model.reflectores;
using apigenerica.model.servicios;
using comunes.primitivas;
using Microsoft.Extensions.Logging;
using pika.modelo.organizacion.Contacto;
using pika.servicios.organizacion.dbcontext;

namespace pika.servicios.organizacion.contacto.redsocial;

[ServicioCatalogoEntidadAPI(typeof(TipoRedSocial))]
public class ServicioTipoRedSocial : ServicioCatalogoGenericoBase, IServicioCatalogoAPI, IServicioTipoRedSocial
{
    public ServicioTipoRedSocial(ILogger<ServicioTipoRedSocial> logger, DbContextOrganizacion context) : base(context, context.TipoRedSocial, logger)
    {
    }

    public bool RequiereAutenticacion => true;

    public string IdiomaDefault => "es-MX";

    public List<ElementoCatalogo> ElementosDefault()
    {
        return new List<ElementoCatalogo>()
        {

        };
    }


    #region Override

    private DbContextOrganizacion DB { get { return (DbContextOrganizacion)_db; } }


    #endregion

}

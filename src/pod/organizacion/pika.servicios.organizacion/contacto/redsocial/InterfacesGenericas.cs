using apigenerica.model.reflectores;
using apigenerica.model.servicios;
using pika.modelo.organizacion;
using pika.modelo.organizacion.Contacto;

namespace pika.servicios.organizacion.contacto.redsocial;

public interface IServicioRedSocial : IServicioEntidadGenerica<RedSocial, RedSocialInsertar, RedSocialActualizar, RedSocialDespliegue, string>
{
}

public interface IServicioTipoRedSocial : IServicioCatalogoAPI
{

}

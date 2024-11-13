using apigenerica.model.servicios;
using comunes.primitivas;
using pika.modelo.organizacion;
using System.Collections.Specialized;

namespace pika.servicios.organizacion.unidadorganizacional;

public interface IServicioUnidadOrganizacional : IServicioEntidadGenerica<UnidadOrganizacional, UnidadOrganizacionalInsertar, UnidadOrganizacionalActualizar, UnidadOrganizacionalDespliegue, string>
{
}
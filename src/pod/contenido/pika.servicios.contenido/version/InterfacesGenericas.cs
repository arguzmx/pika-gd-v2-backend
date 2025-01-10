using apigenerica.model.servicios;
using pika.modelo.contenido;

namespace pika.servicios.contenido.version;

public interface IServicioVersion : IServicioEntidadGenerica<EntidadVersion, VersionInsertar, VersionActualizar, VersionDespliegue, string>
{
}

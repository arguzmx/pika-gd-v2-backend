using apigenerica.model.servicios;
using comunes.primitivas;
using pika.modelo.contenido;
using System.Collections.Specialized;

namespace pika.servicios.contenido.version;

public interface IServicioVersion : IServicioEntidadGenerica<EntidadVersion, VersionInsertar, VersionActualizar, VersionDespliegue, string>
{
    Task<ResultadoValidacion> ValidarRepoCOntenido(StringDictionary parametros);
}

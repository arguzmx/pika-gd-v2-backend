using apigenerica.model.servicios;
using pika.modelo.contenido;

namespace pika.servicios.contenido;

/// <summary>
/// Interface servicio carpeta
/// </summary>
public interface IServicioCarpeta : IServicioEntidadGenerica<Carpeta, CarpetaInsertar, CarpetaActualizar, CarpetaDespliegue, string>
{
}

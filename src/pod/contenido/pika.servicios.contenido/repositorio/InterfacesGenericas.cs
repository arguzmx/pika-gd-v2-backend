using apigenerica.model.servicios;
using pika.modelo.contenido;

namespace pika.servicios.contenido;

/// <summary>
/// Interface servicio repositorio
/// </summary>
public interface IServicioRepositorio : IServicioEntidadGenerica<Repositorio, RepositorioInsertar, RepositorioActualizar, RepositorioDespliegue, string>
{
}
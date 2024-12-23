using apigenerica.model.interpretes;
using apigenerica.model.modelos;
using apigenerica.model.reflectores;
using apigenerica.model.servicios;
using comunes.primitivas;
using extensibilidad.metadatos;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using pika.modelo.contenido;
using pika.servicios.contenido.dbcontext;
using System.Collections.Specialized;
using System.Text.Json;

namespace pika.servicios.contenido;

/// <summary>
/// Interface servicio contenido
/// </summary>
public interface IServicioContenido : IServicioEntidadGenerica<Contenido, ContenidoInsertar, ContenidoActualizar, ContenidoDespliegue, string>
{
}

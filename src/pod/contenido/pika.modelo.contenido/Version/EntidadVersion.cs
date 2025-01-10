using CouchDB.Driver.Types;
using extensibilidad.metadatos.atributos;
using Newtonsoft.Json;

namespace pika.modelo.contenido;

/// <summary>
/// Define una coleccion de archivos y propiedades que definen una version de un contenido especifico
/// </summary>
[Entidad()]
public class EntidadVersion : CouchDocument
{

    /// COmo hereda de CouchDocument hay una propiedad Id 
    /// Heredada y que debera inicializarse con un Id unico

    /// <summary>
    /// Identificador del repositorio al que pertenece el contenido
    /// </summary>
    [JsonProperty("cid")]
    public required string RepositorioId { get; set; }


    /// <summary>
    /// Identificador del contendido al que pertenece la version
    /// </summary>
    [JsonProperty("cid")]
    public required string ContenidoId { get; set; }

    /// <summary>
    /// Indica si la versión es la activa, sólo pude existir una versión activa por elemento
    /// </summary>
    [JsonProperty("a")]
    public bool? Activa { get; set; } = true;

    /// <summary>
    /// Fecha de ceración de la versión
    /// </summary>
    [JsonProperty("fc")]
    public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Fecha de ceración de la versión
    /// </summary>
    [JsonProperty("fc")]
    public DateTime FechaActualizacion { get; set; } = DateTime.UtcNow;


    /// <summary>
    /// Mantiene la cuenta del número de partes asociadas a la versión
    /// </summary>
    [JsonProperty("p")]
    public int ConteoPartes { get; set; } = 0;

    /// <summary>
    /// Mantiene el tamaño en bytes de las partes de la versión
    /// </summary>
    [JsonProperty("t")]
    public long TamanoBytes { get; set; } = 0;

    /// <summary>
    /// Identificador único del volumen para la version
    /// </summary>
    [JsonProperty("vid")]
    public required string VolumenId { get; set; }

    /// <summary>
    /// LIsta de anexos asociados a la versión de contenido
    /// </summary>
    [JsonProperty("as")]
    public List<Anexo> Anexos { get; set; } = [];

}

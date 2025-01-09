using extensibilidad.metadatos.atributos;
using Newtonsoft.Json;

namespace pika.modelo.contenido;

/// <summary>
/// Deine un archivo anexo asociado a un aversión de contenido
/// </summary>
public class Anexo
{

    /// <summary>
    /// Identificador único del anexo
    /// </summary>
    [JsonProperty("id")]
    [Formulario(indice: 1, visible: false)]
    [Tabla(indice: 0, visible: false)]
    public string Id { get; set; }

    /// <summary>
    /// Posición relativa de la parte dentro del elemento de contenido
    /// </summary>
    [JsonProperty("i")]
    public int Indice { get; set; }

    /// <summary>
    /// Consecutivo del elemento para el alamcenamiento, esta propieda tambié existe en el Volumen 
    /// para poder asociar un Id de tipo String, con uno númerico si es necesario
    /// </summary>
    [JsonProperty("cv")]
    public long ConsecutivoVolumen { get; set; } = 0;

    /// <summary>
    /// Tipo MIME asociado al contenido
    /// </summary>
    [JsonProperty("m")]
    public string TipoMime { get; set; }


    /// <summary>
    /// Extensión del archivo recibido
    /// </summary>
    [JsonProperty("ex")]
    public string Extension { get; set; }


    /// <summary>
    /// Identificador externo de la parte, por ejemplo al importar de otros sistemas
    /// </summary>
    [JsonProperty("iex")]
    public string? IdentificadorExterno { get; set; }

    /// <summary>
    /// Longidut en bytes de la parte
    /// </summary>
    [JsonProperty("b")]
    public long LongitudBytes { get; set; } = 0;

    /// <summary>
    /// Nombre original del anexo, corresponde con el nombre del archivo electrónico
    /// </summary>
    [JsonProperty("n")]
    public string? NombreOriginal { get; set; }

    /// <summary>
    /// Indica si el ekemebnto ha sido marcado para eliminar, en el caso de las partes el proceso de eliminación 
    /// es asíncrono basado en esta propiedad
    /// </summary>
    [JsonProperty("elim")]
    public bool Eliminada { get; set; }

    /// <summary>
    /// Indica si la parte es del tipo imagen
    /// </summary>
    [JsonProperty("img")]
    public bool EsImagen { get; set; }

    /// <summary>
    /// Indica si la parte es del tipo audio
    /// </summary>
    [JsonProperty("aud")]
    public bool EsAudio { get; set; }

    /// <summary>
    /// Indica si la parte es del tipo video
    /// </summary>
    [JsonProperty("vid")]
    public bool EsVideo { get; set; }

    /// <summary>
    /// Indica si la parte es del tipo video
    /// </summary>
    [JsonProperty("pdf")]
    public bool EsPDF { get; set; }

    /// <summary>
    /// Indica si la parte tiene una miniatura generada
    /// </summary>
    [JsonProperty("min")]
    public bool TieneMiniatura { get; set; }

    /// <summary>
    /// Determina si el contenido ha sido indexado
    /// </summary>
    [JsonProperty("idx")]
    public bool Indexada { get; set; }
}

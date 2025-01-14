using comunes.primitivas.atributos;
using CouchDB.Driver.Types;
using extensibilidad.metadatos.atributos;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Serialization;

namespace pika.modelo.contenido;

public class Contenido
{
    /// <summary>
    /// Indetificador único del elemento de contenido
    /// </summary>
    [Id]
    [Formulario(indice: 1, visible: false)]
    [Tabla(indice: 0, visible: false)]
    public string Id { get; set; }
    // [a] [d] 
    // R 128

    /// <summary>
    /// Nombre común dado al elemento de contenido
    /// </summary>
    [Nombre]
    [Formulario(indice: 1, ancho: 100)]
    [Tabla(indice: 1)]
    public string Nombre { get; set; }
    // [I ][a] [d] 
    // R 500

    /// <summary>
    /// Identificador del punto de montaje asociado a la carpeta
    /// </summary>
    [Tabla(indice: 0, visible: true)]
    [UsoCatalogoAttribute("Repositorio", true)]
    public string RepositorioId { get; set; }
    // [i] 
    // R 128

    /// <summary>
    /// Identificador único del creador de la carpeta
    /// </summary>
    public string CreadorId { get; set; }
    // [d] Este valor simpre viene del contexto del id en el JWT
    // R 128

    /// <summary>
    /// FEcha de creación en formato UTC
    /// </summary>
    public DateTime FechaCreacion { get; set; }
    // [d]
    // Este valor viene del contexto se crea de la fecha del sistema al insertar y sólo se despliega

    /// <summary>
    /// Número de adjuntos en la vesion actual del contenido
    /// </summary>
    public int ConteoAnexos { get; set; } = 0;
    // [d]
    // Este valor viene del contexto se actualiza con el CRUD de anexos del contenido


    /// <summary>
    /// Tamaño en bytes del total de anexos en la versión actual del contenido
    /// </summary>
    public long TamanoBytes { get; set; } = 0;
    // [d]
    // Este valor viene del contexto se actualiza con el CRUD de anexos del contenido

    /// <summary>
    /// IDentificador único del volumen al que se añadió el contenido
    /// </summary>
    [Tabla(indice: 0, visible: true)]
    [UsoCatalogoAttribute("Volumen", true)]
    public string VolumenId { get; set; }
    // [i] [d] 
    // R 128

    /// <summary>
    /// Identificador de la carpeta donde se creó el contenido
    /// </summary>
    [Tabla(indice: 0, visible: true)]
    [UsoCatalogoAttribute("Carpeta", true)]
    public string CarpetaId { get; set; }
    // [i] [d] 
    // R 128

    /// <summary>
    /// Identifica el tipo del elemento mostrado, nulo para tipo defautl
    /// o un tipo asociado al visor por ejemplo, expediente, cfdi, entro otros
    /// puede utilizarse también para determinarl el ícono del elemento
    /// </summary>
    public string TipoElemento { get; set; }
    // [i] [d] 
    // R 128

    /// <summary>
    /// Esta Identificador permite asociar el elemento a un sistema externo
    /// como clave de búsqueda
    /// </summary>
    public string IdExterno { get; set; }
    // [i] [a] [d] 
    // R 128

    /// <summary>
    /// Identificador del permiso asociado al contenido, null por defaulr
    /// </summary>
    public string? PermisoId { get; set; }
    // 128 esta propiedad se va a llenar en una operacion especial

    // Propieades de navegación

    //[XmlIgnore, JsonIgnore]
    //public Permiso? Permiso { get; set; }

    [XmlIgnore]
    [JsonIgnore]
    public Volumen Volumen { get; set; }

    [XmlIgnore]
    [JsonIgnore]
    public Repositorio Repositorio { get; set; }

    [XmlIgnore]
    [JsonIgnore]
    public Carpeta Carpeta { get; set; }


    [XmlIgnore]
    [JsonIgnore]
    [NotMapped]

    public List<EntidadVersion> Versiones { get; set; }
}

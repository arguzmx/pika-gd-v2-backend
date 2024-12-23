using extensibilidad.metadatos.atributos;
using System.Text.Json.Serialization;
using System.Xml.Serialization;

namespace pika.modelo.contenido;

/// <summary>
/// Representa una carpeta para el arrglo lógico del conteido
/// </summary>
[Entidad()]
public class Carpeta
{
    /// <summary>
    ///  Identificdor únio del volumen
    ///  Se obtiene con GUID new
    /// </summary>
    [Id]
    [Formulario(indice: 1, visible: false)]
    [Tabla(indice: 0, visible: false)]
    public string Id { get; set; }
    // [a] [d] 
    // R 128

    /// <summary>
    /// Identificador del punto de montaje asociado a la carpeta
    /// </summary>
    [Tabla(indice: 0, visible: true)]
    [UsoCatalogo("Repositorio", true)]
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
    // Este valor simpre viene del contexto se crea de la fecha del sistema al insertar y sólo se despliega


    /// <summary>
    /// Nombre para la carpeta 
    /// </summary>
    [Nombre]
    [Formulario(indice: 1, ancho: 100)]
    [Tabla(indice: 1)]
    public string Nombre { get; set; }
    // [i] [a] [d]
    // R 512

    /// <summary>
    /// Identificador de la carpeta padre de la actual 
    /// </summary>
    public string? CarpetaPadreId { get; set; }
    // [i] [a] [d]
    // 128 

    /// <summary>
    /// Determina si la carpeta es un nodo raíz
    /// </summary>
    public bool EsRaiz { get; set; }
    // Se calcula automaticamente, si carpeta padre id es nulo entonces EsRaiz = true, false en caso contrario

    /// <summary>
    /// Identificador del permiso asociado a la carpeta, null por default
    /// </summary>
    public string? PermisoId { get; set; }
    // 128 esta propiedad se va a llenar en una operacion especial

    //// Propiedades de navegación

    //[XmlIgnore, JsonIgnore]
    //public Permiso? Permiso { get; set; }

    ///// <summary>
    ///// Popieadd de navegacion para RepositorioId
    ///// </summary>
    //[XmlIgnore]
    //[JsonIgnore]
    //public Repositorio Repositorio { get; set; }

    /// <summary>
    /// Contenidos asociados a la carpeta
    /// </summary>
    public List<Contenido> Contenido { get; set; }

}

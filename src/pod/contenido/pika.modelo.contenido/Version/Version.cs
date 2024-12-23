using CouchDB.Driver.Types;
using extensibilidad.metadatos.atributos;

namespace pika.modelo.contenido;


//[Entidad()]
//public class Version : CouchDocument
//{

//    /// <summary>
//    /// Identificador único del elemento al que pertenece la versión
//    /// </summary>
//    public string ContenidoId { get; set; }
//    // [i] [d] 
//    // R 128

//    /// <summary>
//    /// Indica si la versión es la activa, sólo pude existir una versión activa por elemento
//    /// </summary>
//    public bool Activa { get; set; } = true;
//    // [i] [a] [d] 
//    // R 

//    /// <summary>
//    /// Fecha de ceración de la versión
//    /// </summary>
//    public DateTime FechaCreacion { get; set; }
//    // [d] Se calcula automaticamente al crear la entidad con Datetime.UTCnow
//    // R 

//    /// <summary>
//    /// Identificadro único del creador de la versión
//    /// </summary>
//    public string CreadorId { get; set; }
//    // [d] Se calcula automaticamente con el is del usuario en el JWT
//    // R 

//    /// <summary>
//    /// Mantiene la cuenta del número de partes asociadas a la versión
//    /// </summary>
//    public int ConteoPartes { get; set; } = 0;
//    // [d] Se calcula automaticamente con el crud de partes
//    // R 


//    /// <summary>
//    /// Mantiene el tamaño en bytes de las partes de la versión
//    /// </summary>
//    public long TamanoBytes { get; set; } = 0;
//    // [d] Se calcula automaticamente con el crud de partes
//    // R 

//    /// <summary>
//    /// Identificador único del volumen para la version
//    /// </summary>
//    public string VolumenId { get; set; }
//    // [i] [d] 
//    // R 128


//    /// <summary>
//    /// LIsta de anexos asociados a la versión de contenido
//    /// </summary>
//    public List<Anexo> Anexos { get; set; } = new List<Anexo>();
//    // Se actualiza vía el CRUD de Anexos

//    //[XmlIgnore]
//    //[JsonIgnore]
//    //public Contenido Contenido { get; set; }

//    //[XmlIgnore]
//    //[JsonIgnore]
//    //public Volumen Volumen { get; set; }

//}

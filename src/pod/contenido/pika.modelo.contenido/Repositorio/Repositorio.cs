using extensibilidad.metadatos.atributos;
using System.Text.Json.Serialization;
using System.Xml.Serialization;

namespace pika.modelo.contenido;

/// <summary>
/// Representa un repositorio para el almacenamiento y clasificación de contenido
/// </summary>
[Entidad()]
public class Repositorio
{
    /// <summary>
    ///  Identificdor únio del repositorio
    ///  Se obtiene con GUID new
    /// </summary>
    [Id]
    [Formulario(indice: 1, visible: false)]
    [Tabla(indice: 0, visible: false)]
    public string Id { get; set; }
    // [a] [d] 
    // R 128

    /// <summary>
    /// Dominio al que pertenece el repositorio
    /// </summary>
    [Protegido]
    public string DominioId { get; set; }
    // Este valor simpre viene del contexto
    // R 128

    /// <summary>
    /// Unidad organizacional a la que pertenece el repositorio
    /// </summary>
    [Protegido]
    public string UOrgId { get; set; }
    //  Este valor simpre viene del contexto
    // R 128

    /// <summary>
    /// Nombre del repositorio 
    /// </summary>
    [Nombre]
    [Formulario(indice: 1, ancho: 100)]
    [Tabla(indice: 1)]
    public string Nombre { get; set; }
    // [i] [a] [d]
    // R 500

    [XmlIgnore]
    [JsonIgnore]
    public List<Carpeta> Carpetas { get; set; }


    /// <summary>
    /// Contenidos asociados al respositorio
    /// </summary>
    public List<Contenido> Contenido { get; set; }
}

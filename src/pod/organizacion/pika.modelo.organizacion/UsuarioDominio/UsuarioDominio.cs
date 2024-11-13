using comunes.primitivas.atributos;
using extensibilidad.metadatos.atributos;
using System.Text.Json.Serialization;
using System.Xml.Serialization;

namespace pika.modelo.organizacion;


/// <summary>
/// Relaciona un usuario con su dominio, como esta entidad solo almacena solo ids 
/// unicamente acepta metodos de insercion y elimminaci[on
/// </summary>
[EntidadDB()]
public class UsuarioDominio
{

    /// <summary>
    /// Identificador único de la relacióm
    /// </summary>
    [Id]
    [Formulario(indice: 1, visible: false)]
    [Tabla(indice: 0, visible: false)]
    public string Id { get; set; }
    // [d]
    // R [128]

    /// <summary>
    /// Identificador único del usuario, este viene del servicio de identidad
    /// </summary>
    public string UsuarioId { get; set; }
    // [i] [d]
    // R [128]

    /// <summary>
    /// Identificador único del dominio
    /// </summary>
    [Protegido]
    public string DominioId { get; set; }
    // [i] [d]
    // R [128]

    [JsonIgnore]
    [XmlIgnore]
    public Dominio Dominio { get; set; }

}

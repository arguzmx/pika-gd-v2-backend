using comunes.primitivas.atributos;
using extensibilidad.metadatos.atributos;
using System.Text.Json.Serialization;
using System.Xml.Serialization;

namespace pika.modelo.organizacion;
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

/// <summary>
/// Relaciona un usuario con una unidad organizacional, como soolo almacenad ids 
/// requiere implementar unicamente las opciones de insertat y eleiminar
/// </summary>
/// 
[EntidadDB()]
public class UsuarioUnidadOrganizacional
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


    /// <summary>
    /// Identificador único de la unidad organizaciona, este valor se obtiene del encabezado 
    /// </summary>
    [Tabla(indice: 0, visible: true)]
    [UsoCatalogoAttribute("UnidadOrganizacional", true)]
    public string UnidadOrganizacionalId { get; set; }


    [JsonIgnore]
    [XmlIgnore]
    public UnidadOrganizacional UnidadOrganizacional { get; set; }

    [JsonIgnore]
    [XmlIgnore]
    public Dominio Dominio { get; set; }
}
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.


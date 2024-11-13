using comunes.primitivas.atributos;
using extensibilidad.metadatos.atributos;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;
using System.Xml.Serialization;

namespace pika.modelo.organizacion;

/// <summary>
/// Las unidades organizacionales agrupan recursos para la organización del trabajo
/// </summary>
/// 
[EntidadDB]
[ExcludeFromCodeCoverage]
public class UnidadOrganizacional
{

    /// <summary>
    /// Identificador único de la UI
    /// </summary>
    [Id]
    [Formulario(indice: 1, visible: false)]
    [Tabla(indice: 0, visible: false)]
    public string Id { get; set; }

    /// <summary>
    /// NOmbre de la unodad organizacional
    /// </summary>
    [Nombre]
    [Formulario(indice: 1, ancho: 100)]
    [Tabla(indice: 1)]
    public string Nombre {get; set;}

    /// <summary>
    /// Identiicador único del dominio al que se asocia la UO, el sominio se lee del header de dominio especifico
    /// </summary>
    [Protegido]
    public string DominioId { get; set; }


    [JsonIgnore]
    [XmlIgnore]
    public Dominio Dominio { get; set; }


    [XmlIgnore]
    [JsonIgnore]
    public List<UsuarioUnidadOrganizacional> UsuariosUnidad { get; set; }
}

using comunes.primitivas.atributos;
using extensibilidad.metadatos.atributos;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;
using System.Xml.Serialization;

namespace pika.modelo.organizacion;

/// <summary>
/// Un dominio de datos es un contenedor lógico para todos los elementos relaciondso 
/// con una instancia de gestión documental
/// </summary>
/// 
[EntidadDB]
[ExcludeFromCodeCoverage]
public class Dominio
{
    /// <summary>
    ///  Identificdor únio del dominio
    ///  Se obtiene con GUID new
    /// </summary>
    [Id]
    [Formulario(indice: 1, visible: false)]
    [Tabla(indice: 0, visible: false)]
    public string Id { get; set; }
    // [a] [d] 
    // R 128

    /// <summary>
    /// Nombre único del dominio
    /// </summary>
    [Nombre]
    [Formulario(indice: 1, ancho: 100)]
    [Tabla(indice: 1)]
    public string Nombre { get; set; }
    // [i] [a]
    // R 500

    /// <summary>
    /// Determina si el dominio se encuentra activo
    /// </summary>
    public bool Activo { get; set; } = true;
    // [i] [a]
    // R 

    /// <summary>
    /// Fecha de creacion del dominio, se calcula automaticamente al crear la entidad para insercion
    /// </summary>
    public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Identificador unico del usuario dueño del dominio, este nombre se calcula con el ID del usaurio en sesion
    /// al momento de crear el dominio en la API
    /// </summary>
    [Tabla(indice: 0, visible: true)]
    [UsoCatalogoAttribute("Usuario", true)]
    public string UsuarioId { get; set; }


    [XmlIgnore]
    [JsonIgnore]
    public List<UsuarioDominio> UsuarioDominio { get; set; } = new List<UsuarioDominio>();

    [XmlIgnore]
    [JsonIgnore]
    public List<UnidadOrganizacional> UnidadesOrganizacionales { get; set; } = new List<UnidadOrganizacional>();

    [XmlIgnore]
    [JsonIgnore]
    public List<UsuarioUnidadOrganizacional> UsuarioUnidadOrganizacionals { get; set; } = new List<UsuarioUnidadOrganizacional>();
}
using extensibilidad.metadatos.atributos;
using System.Text.Json.Serialization;
using System.Xml.Serialization;

namespace pika.modelo.organizacion.Contacto;

/// <summary>
/// Red social de contacto
/// </summary>
public class RedSocial
{ 
    /// <summary>
    /// Identificador único del número de teléfono
    /// </summary>
    [Id]
    [Formulario(indice: 1, visible: false)]
    [Tabla(indice: 0, visible: false)]
    public string Id { get; set; }
    // [a] [d] 
    // R 128

    /// <summary>
    /// URL o identificador de la red
    /// </summary>
    [Nombre]
    [Formulario(indice: 1, ancho: 500)]
    [Tabla(indice: 1)]
    public string Url { get; set; }
    // [i] [a] [d] 
    // R 20

    /// <summary>
    /// Tipo de archivo del catálogo
    /// </summary>
    //[UsoCatalogo(idCatalogo: nameof(TipoRedSocial))]
    [Tabla(indice: 0, visible: true)]
    [UsoCatalogoAttribute("TipoRedSocial", true)]
    public string TipoRedSocialId { get; set; }
    // [i] [a]
    // R 128


    /// <summary>
    /// Dominio al que pertenece el archivo
    /// </summary>
    [Protegido]
    public string DominioId { get; set; }
    // Este valor simpre viene del contexto
    // R 128

    /// <summary>
    /// Unidad organizacional a la que pertenece el archivo
    /// </summary>
    [Protegido]
    public string UOrgId { get; set; }
    //  Este valor simpre viene del contexto
    // R 128



    [JsonIgnore]
    [XmlIgnore]
    public TipoRedSocial TipoRedSocial { get; set; }

}

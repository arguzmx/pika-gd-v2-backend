using extensibilidad.metadatos.atributos;
using System.Xml.Serialization;

namespace pika.modelo.gestiondocumental;
/// <summary>
/// Define la unidad adminsirtativa donde los usuarios que no forman parte del grupo de archivónomos mantienen el inventario de trámite
/// </summary>
public class UnidadAdministrativa
{

    /// <summary>
    /// Identificador único de la unidad administrativa
    /// </summary>
    [Id]
    [Formulario(indice: 1, visible: false)]
    [Tabla(indice: 0, visible: false)]
    public string Id { get; set; }

    /// <summary>
    /// NOmbre de la unidad administrativa
    /// </summary>
    [Nombre]
    [Formulario(indice: 1, ancho: 100)]
    [Tabla(indice: 1)]
    public string Nombre { get; set; }
    //R [500] I A D

    /// <summary>
    /// Nombre del responsable de la unidad
    /// </summary>
    public string Responsable { get; set; }
    //R [500] I A D


    /// <summary>
    /// CArgo del responsable de la unidad
    /// </summary>
    public string Cargo { get; set; }
    //R [500] I A D

    /// <summary>
    /// TElédono de contacto de la unidad
    /// </summary>
    public string Telefono { get; set; }
    //R [50] I A D

    /// <summary>
    /// Email de contacto de la unidad
    /// </summary>
    public string Email { get; set; }
    //R [50] I A D


    /// <summary>
    /// Domicilio de la unidad
    /// </summary>
    public string Domicilio { get; set; }
    //R [200] I A D

    /// <summary>
    /// Ubicación de la unidad en el domicilio
    /// </summary>
    public string UbicacionFisica { get; set; }
    //R [200] I A D

    /// <summary>
    /// Identificador único del archivo de trámite donde se crearán los activos del acervo
    /// </summary>
    [Tabla(indice: 0, visible: true)]
    [UsoCatalogoAttribute("ArchivoTramite", true)]
    public string ArchivoTramiteId { get; set; }
    // I [a] [d] 
    // R 128

    /// <summary>
    /// Identificador único del archivo de concentración donde se crearán los activos del acervo
    /// </summary>
    [Tabla(indice: 0, visible: true)]
    [UsoCatalogoAttribute("ArchivoConcentracion", true)]
    public string ArchivoConcentracionId { get; set; }
    // I [a] [d] 
    // R 128

    /// <summary>
    /// Identificador único del archivo histórico donde se crearán los activos del acervo
    /// </summary>
    [Tabla(indice: 0, visible: true)]
    [UsoCatalogoAttribute("ArchivoHistorico", true)]
    public string ArchivoHistoricoId { get; set; }
    // I [a] [d] 
    // R 128

    //[JsonIgnore]
    //[XmlIgnore]
    //public Archivo ArchivoTramite { get; set; }

    //[JsonIgnore]
    //[XmlIgnore]
    //public Archivo ArchivoConcentracion { get; set; }

    //[JsonIgnore]
    //[XmlIgnore]
    //public Archivo ArchivoHistorico { get; set; }

    //[JsonIgnore]
    //[XmlIgnore]
    //public ICollection<Activo> Activos { get; set; }

    //[JsonIgnore]
    //[XmlIgnore]
    //public ICollection<PermisosUnidadAdministrativaArchivo> Permisos { get; set; }

    //[JsonIgnore]
    //[XmlIgnore]
    //public List<EstadisticaClasificacionAcervo> EstadisticasClasificacionAcervo { get; set; }
}

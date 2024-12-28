using extensibilidad.metadatos.atributos;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;
using System.Xml.Serialization;

namespace pika.modelo.gestiondocumental;

/// <summary>
/// REpresenta una seria documental de un cuadro de clasificación
/// </summary>
[ExcludeFromCodeCoverage]
public class SerieDocumental
{
    /// <summary>
    /// Identificador único de la serie
    /// </summary>
    [Id]
    [Formulario(indice: 1, visible: false)]
    [Tabla(indice: 0, visible: false)]
    public string Id { get; set; }
    // [a] [d] 
    // R 128

    /// <summary>
    /// Identificador punico del cuadro de clasificación al que pertenece la serie
    /// </summary>
    [Tabla(indice: 0, visible: true)]
    [UsoCatalogoAttribute("CuadroClasificacion", true)]
    public string CuadroClasificacionId { get; set; }
    // [i] [d]
    // R 128


    /// <summary>
    /// Clave de la serie documental por ejemplo 1.C.03
    /// </summary>
    public string Clave { get; set; }
    // [i] [a] [d]
    // R 200

    /// <summary>
    /// Nombre de la serie documental
    /// </summary>
    [Nombre]
    [Formulario(indice: 1, ancho: 100)]
    [Tabla(indice: 1)]
    public string Nombre { get; set; }
    // [i] [a] [d]
    // R 500

    /// <summary>
    /// Determina si la serie es el inicio de la estructura jerárquica
    /// </summary>
    public bool Raiz { get; set; }
    // [i] [a] [d]
    // R 

    /// <summary>
    /// Identificador de la seria padre
    /// </summary>
    [Tabla(indice: 0, visible: true)]
    [UsoCatalogoAttribute("SeriePadre", true)]
    public string? SeriePadreId { get; set; }
    // [i] [a] [d]
    // R 128


    /// <summary>
    /// MEses para conclusión en archivo de trámite
    /// </summary>
    public int MesesArchivoTramite { get; set; }
    // [i] [a] [d]
    // R 

    /// <summary>
    /// MEses para conclusión en archivo de concentración
    /// </summary>
    public int MesesArchivoConcentracion { get; set; }
    // [i] [a] [d]
    // R 


    /// <summary>
    /// MEses para conclusión en archivo de histórico
    /// </summary>
    public int MesesArchivoHistorico { get; set; }
    // [i] [a] [d]
    // R


    /// <summary>
    /// Cuadro de clasificación al que pertenece la serie
    /// </summary>
    [XmlIgnore, JsonIgnore]
    public CuadroClasificacion CuadroClasificacion { get; set; }


    /// <summary>
    /// Seria padre en el  modelo jerárquico
    /// </summary>
    [XmlIgnore, JsonIgnore]
    public SerieDocumental? SeriePadre { get; set; }


    /// <summary>
    /// Series docuentales descendientes
    /// </summary>
    public List<SerieDocumental> Subseries { get; set; } = new List<SerieDocumental>();


    ///// <summary>
    ///// Seria padre en el  modelo jerárquico
    ///// </summary>
    //[XmlIgnore, JsonIgnore]
    //public List<Transferencia>? Transferencias { get; set; }

}
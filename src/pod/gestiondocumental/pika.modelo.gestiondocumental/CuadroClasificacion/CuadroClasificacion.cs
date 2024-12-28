using extensibilidad.metadatos.atributos;
using System.Xml.Serialization;
using System;
using System.Text.Json.Serialization;

namespace pika.modelo.gestiondocumental;
[Entidad()]
public class CuadroClasificacion
{
    /// <summary>
    /// Identificador únioc del cuadro de clasificación
    /// </summary>
    [Id]
    [Formulario(indice: 1, visible: false)]
    [Tabla(indice: 0, visible: false)]
    public string Id { get; set; }
    // [a] [d] 
    // R 128


    /// <summary>
    /// Nombre único del cuadro de clasificación
    /// </summary>
    [Nombre]
    [Formulario(indice: 1, ancho: 100)]
    [Tabla(indice: 1)]
    public string Nombre { get; set; }

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


    ///// <summary>
    ///// Activos que utilizan el cuadro
    ///// </summary>
    //[JsonIgnore, XmlIgnore]
    //public List<Activo> Activos { get; set; }


    /// <summary>
    /// Series documentales pertenecientes al cuadro de clasificación
    /// </summary>
    [JsonIgnore, XmlIgnore]
    public List<SerieDocumental> Series { get; set; }

    ///// <summary>
    ///// Seria padre en el  modelo jerárquico
    ///// </summary>
    //[XmlIgnore, JsonIgnore]
    //public List<Transferencia>? Transferencias { get; set; }

}

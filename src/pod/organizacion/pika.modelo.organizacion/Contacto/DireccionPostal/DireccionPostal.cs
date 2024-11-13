using extensibilidad.metadatos.atributos;

namespace pika.modelo.organizacion.Contacto;

/// <summary>
/// Dirección postal
/// </summary>
public class DireccionPostal
{
    /// <summary>
    /// Identificador único de la dirección
    /// </summary>
    [Id]
    [Formulario(indice: 1, visible: false)]
    [Tabla(indice: 0, visible: false)]
    public string Id { get; set; }
    // [a] [d] 
    // R 128

    /// <summary>
    /// Calle de la dirección 
    /// </summary>
    [Nombre]
    [Formulario(indice: 1, ancho: 100)]
    [Tabla(indice: 1)]
    public string Calle { get; set; }
    // [i] [a] [d] 
    // R 500

    /// <summary>
    /// No interior la dirección 
    /// </summary>
    [Nombre]
    [Formulario(indice: 1, ancho: 100)]
    [Tabla(indice: 1)]
    public string NoInterior { get; set; }
    // [i] [a] [d] 
    // R 50

    /// <summary>
    /// No xterior de la dirección 
    /// </summary>
    [Nombre]
    [Formulario(indice: 1, ancho: 100)]
    [Tabla(indice: 1)]
    public string NoExterior { get; set; }
    // [i] [a] [d] 
    // R 50

    /// <summary>
    /// Código postal de la dirección 
    /// </summary>
    [Nombre]
    [Formulario(indice: 1, ancho: 100)]
    [Tabla(indice: 1)]
    public string CP { get; set; }
    // [i] [a] [d] 
    // R 20

    /// <summary>
    /// Pais de la dirección 
    /// </summary>
    [Nombre]
    [Formulario(indice: 1, ancho: 100)]
    [Tabla(indice: 1)]
    public string Pais { get; set; }
    // [i] [a] [d] 
    // R 200

    /// <summary>
    /// Estado de la dirección 
    /// </summary>
    [Nombre]
    [Formulario(indice: 1, ancho: 100)]
    [Tabla(indice: 1)]
    public string Estado { get; set; }
    // [i] [a] [d] 
    // R 200

    /// <summary>
    /// Ciudad de la dirección 
    /// </summary>
    [Nombre]
    [Formulario(indice: 1, ancho: 100)]
    [Tabla(indice: 1)]
    public string Ciudad { get; set; }
    // [i] [a] [d] 
    // R 200

    /// <summary>
    /// INformación de Referencia de la dirección 
    /// </summary>
    [Nombre]
    [Formulario(indice: 1, ancho: 100)]
    [Tabla(indice: 1)]
    public string Referencia { get; set; }
    // [i] [a] [d] 
    // R 500

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
}

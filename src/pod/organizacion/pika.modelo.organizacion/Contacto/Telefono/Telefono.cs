using extensibilidad.metadatos.atributos;

namespace pika.modelo.organizacion.Contacto;

/// <summary>
/// Número telefónico de contacto
/// </summary>
public class Telefono
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
    /// Número telefónico
    /// </summary>
    [Nombre]
    [Formulario(indice: 1, ancho: 100)]
    [Tabla(indice: 1)]
    public string Numero { get; set; }
    // [i] [a] [d] 
    // R 20

    /// <summary>
    /// Extensión en el conmutador
    /// </summary>
    [Nombre]
    [Formulario(indice: 1, ancho: 100)]
    [Tabla(indice: 1)]
    public string Extension { get; set; }
    // [i] [a] [d] 
    // R 200


    /// <summary>
    /// Horario de atención telefónica
    /// </summary>
    [Nombre]
    [Formulario(indice: 1, ancho: 100)]
    [Tabla(indice: 1)]
    public string Horario { get; set; }
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

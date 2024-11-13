using extensibilidad.metadatos.atributos;


namespace pika.modelo.organizacion;

/// <summary>
/// Describe un puesto dentro de la organización
/// </summary>

public class Puesto
{
    /// <summary>
    /// Identificador único del puesto 
    /// </summary>
    [Id]
    [Formulario(indice: 1, visible: false)]
    [Tabla(indice: 0, visible: false)]
    public string Id { get; set; }
    // [a] [d] 
    // R 128

    /// <summary>
    /// Nombre del puesto 
    /// </summary>
    [Nombre]
    [Formulario(indice: 1, ancho: 100)]
    [Tabla(indice: 1)]
    public string Nombre { get; set; }
    // [i] [a] [d] 
    // R 500

    /// <summary>
    /// Clave del puesto 
    /// </summary>
    public string Clave { get; set; }
    // [i] [a] [d] 
    // R 100

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

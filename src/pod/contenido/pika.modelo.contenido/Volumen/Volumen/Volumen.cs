using extensibilidad.metadatos.atributos;

namespace pika.modelo.contenido;

/// <summary>
/// Un volumen de contenido es una ruta de almacenamiento de datos
/// por ejemplo una carpeta en el disco duro 
/// </summary>
[Entidad()]
public class Volumen
{

    /// <summary>
    ///  Identificdor únio del volumen
    ///  Se obtiene con GUID new
    /// </summary>
    [Id]
    [Formulario(indice: 1, visible: false)]
    [Tabla(indice: 0, visible: false)]
    public string Id { get; set; }
    // [a] [d] 
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

    /// <summary>
    /// Nombre único del volumen
    /// </summary>
    [Nombre]
    [Formulario(indice: 1, ancho: 100)]
    [Tabla(indice: 1)]
    public string Nombre { get; set; }
    // [i] [a] [d] 
    // R 500

    /// <summary>
    /// Identificador único del  tipo de gestor, es necesario para la configuración
    /// </summary>
    [Tabla(indice: 0, visible: true)]
    [UsoCatalogoAttribute("TipoGestorEs", true)]
    public string TipoGestorESId { get; set; }
    // [i] [a] 
    // R 128

    /// <summary>
    /// Tamaño maximo del volumen en bytes, 0 indidica ilimitado
    /// </summary>
    public long TamanoMaximo { get; set; }
    // [i] [a] 
    // R

    /// <summary>
    /// Indica si el volumen se encuentra activo 
    /// </summary>
    public bool Activo { get; set; }
    // [i] [a] 
    // R


    /// <summary>
    /// Especifica si el volumen se encuntra habilitado para escritura
    /// </summary>
    public bool EscrituraHabilitada { get; set; }
    // [i] [a] 
    // R

    /// <summary>
    /// Consecutivo del elemento para el alamcenamiento, esta propieda tambié existe en la Parte del contenido 
    /// para poder asociar un Id de tipo String, con uno númerio si es necesario
    /// </summary>
    public long ConsecutivoVolumen { get; set; } = 0;
    // Esta valos se calcula por el sistema
    // R 


    /// <summary>
    /// Número de partess contenidas en el volumen
    /// </summary>
    public long CanidadPartes { get; set; } = 0;
    // Esta valos se calcula por el sistema
    // R 

    /// <summary>
    /// Número de elementos contenidas en el volumen
    /// </summary>
    public long CanidadElementos { get; set; } = 0;
    // Esta valos se calcula por el sistema
    // R 


    /// <summary>
    /// Tamaño actual del volumen en bytes
    /// </summary>
    public long Tamano { get; set; } = 0;
    // Esta valos se calcula por el sistema
    // R 


    /// <summary>
    /// Atributo de uso interno que indica si la configuración del volumen es válida
    /// </summary>
    public bool ConfiguracionValida { get; set; } = false;
    // Esta valos se calcula por el sistema
    // R 


    ///// <summary>
    ///// Tipo de gestor de enntrada salida asociado al volumen
    ///// </summary>
    //public TipoGestorES? TipoGestorES { get; set; }


    /// <summary>
    /// Repositorios asociados al volumen 
    /// </summary>
    public List<Repositorio> Repositorios { get; set; }

    /// <summary>
    /// Contenidos asociados al volumen
    /// </summary>
    public List<Contenido> Contenido { get; set; }

}

namespace pika.modelo.contenido;
public class VersionDespliegue
{
    /// <summary>
    /// Identificaor unico de la version
    /// </summary>
    public string Id { get; set; }

    /// <summary>
    /// Identificador del contendido al que pertenece la version
    /// </summary>
    public required string ContenidoId { get; set; }

    /// <summary>
    /// Indica si la versión es la activa, sólo pude existir una versión activa por elemento
    /// </summary>
    public bool? Activa { get; set; } = true;

    /// <summary>
    /// Fecha de ceración de la versión
    /// </summary>
    public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Fecha de ceración de la versión
    /// </summary>
    public DateTime FechaActualizacion { get; set; } = DateTime.UtcNow;


    /// <summary>
    /// Mantiene la cuenta del número de partes asociadas a la versión
    /// </summary>
    public int ConteoPartes { get; set; } = 0;

    /// <summary>
    /// Mantiene el tamaño en bytes de las partes de la versión
    /// </summary>
    public long TamanoBytes { get; set; } = 0;

    /// <summary>
    /// Identificador único del volumen para la version
    /// </summary>
    public required string VolumenId { get; set; }
}

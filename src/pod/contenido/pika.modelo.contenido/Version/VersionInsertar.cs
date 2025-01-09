using System.Diagnostics.CodeAnalysis;

namespace pika.modelo.contenido;

/// <summary>
/// DTO para la ceacion de una version
/// </summary>
[ExcludeFromCodeCoverage]
public class VersionInsertar
{
    /// <summary>
    /// Identificador del repositorio al que pertenece el contenido
    /// </summary>
    public required string RepositorioId { get; set; }

    /// <summary>
    /// Identificador del contendido al que pertenece la version
    /// </summary>
    public required string ContenidoId { get; set; }

    /// <summary>
    /// Indica si la versión es la activa, sólo pude existir una versión activa por elemento
    /// </summary>
    public bool? Activa { get; set; }

    /// <summary>
    /// Identificador único del volumen para la version
    /// </summary>
    public required string VolumenId { get; set; }


}
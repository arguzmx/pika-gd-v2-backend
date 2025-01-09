using System.Diagnostics.CodeAnalysis;

namespace pika.modelo.contenido;

/// <summary>
/// DTO para la actualizacion de una version
/// </summary>
[ExcludeFromCodeCoverage]
public class VersionActualizar
{
    /// <summary>
    /// Identificaor unico de la version
    /// </summary>
    public string Id { get; set; }


    /// <summary>
    /// Indica si la versión es la activa, sólo pude existir una versión activa por elemento
    /// </summary>
    public bool? Activa { get; set; }

}

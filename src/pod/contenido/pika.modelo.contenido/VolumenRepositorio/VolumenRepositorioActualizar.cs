
using System.Diagnostics.CodeAnalysis;

namespace pika.modelo.contenido;

/// <summary>
/// DTO de inserción para el volumen repositorio
/// </summary>
[ExcludeFromCodeCoverage]
public class VolumenRepositorioActualizar
{
    /// <summary>
    /// Identificador del repositorio
    /// </summary>
    public required string RepositorioId { get; set; }

    /// <summary>
    /// Identificador del volumen
    /// </summary>
    public required string VolumenId { get; set; }

    /// <summary>
    /// Determina si el volumen es el defaul para la gestión de contenido
    /// </summary>
    public bool? Default { get; set; }

    /// <summary>
    /// Determina si el volumen se encuentra activo para el repositorio
    /// </summary>
    public bool? Activo { get; set; } 
}

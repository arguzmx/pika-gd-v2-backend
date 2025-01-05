
using System.Diagnostics.CodeAnalysis;

namespace pika.modelo.contenido;

/// <summary>
/// Relaciones de repositorios con volúmenes para al almacenamiento de contenido
/// </summary>
[ExcludeFromCodeCoverage]
public class VolumenRepositorio
{
    /// <summary>
    /// Identificador único de la relación
    /// </summary>
    public required string Id { get; set; }
    // Longitud 128

    /// <summary>
    /// Identificador del repositorio
    /// </summary>
    public required string RepositorioId { get; set; }
    // INDEXAR
    // Longitud 128

    /// <summary>
    /// Identificador del volumen
    /// </summary>
    public required string VolumenId { get; set; }
    // INDEXAR
    // Longitud 128


    /// <summary>
    /// Determina si el volumen es el default para el contenido del repositorio
    /// </summary>
    public bool Default { get; set; } = false;


    /// <summary>
    /// Determina si el volumen se encuentra activo para el repositorio
    /// </summary>
    public bool Activo { get; set; } = true;
}

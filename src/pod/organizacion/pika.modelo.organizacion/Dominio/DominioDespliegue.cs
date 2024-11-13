using System.Diagnostics.CodeAnalysis;

namespace pika.modelo.organizacion;

[ExcludeFromCodeCoverage]
public class DominioDespliegue
{
    /// <summary>
    ///  Identificdor únio del volumen
    ///  Se obtiene con GUID new
    /// </summary>
    public string Id { get; set; }

    /// <summary>
    /// Nombre único del dominio
    /// </summary>
    public string Nombre { get; set; }

    /// <summary>
    /// Determina si el dominio se encuentra activo
    /// </summary>
    public bool Activo { get; set; } = true;

    /// <summary>
    /// Fecha de creacion del dominio
    /// </summary>
    public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Identificador unico del usuario dueño del dominio
    /// </summary>
    public string UsuarioId { get; set; }

}

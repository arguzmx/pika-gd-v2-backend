using System.Diagnostics.CodeAnalysis;

namespace pika.modelo.organizacion;

[ExcludeFromCodeCoverage]
public class DominioActualizar
{
    /// <summary>
    ///  Identificdor únio del volumen
    ///  Se obtiene con GUID new
    /// </summary>
    public string Id { get; set; }
    /// <summary>
    /// Nombre único del volumen
    /// </summary>
    public string Nombre { get; set; }
    /// <summary>
    /// Determina si el dominio se encuentra activo
    /// </summary>
    public bool Activo { get; set; } = true;
}

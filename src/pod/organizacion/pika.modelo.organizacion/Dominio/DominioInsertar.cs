using System.Diagnostics.CodeAnalysis;

namespace pika.modelo.organizacion;

[ExcludeFromCodeCoverage]
public class DominioInsertar
{
    /// <summary>
    /// Nombre único del volumen
    /// </summary>
    public string Nombre { get; set; }

    public bool Activo { get; set; }

}

using System.Diagnostics.CodeAnalysis;

namespace pika.modelo.organizacion;

[ExcludeFromCodeCoverage]
public class UnidadOrganizacionalInsertar
{
    /// <summary>
    /// NOmbre de la unodad organizacional
    /// </summary>
    public string Nombre { get; set; }

    /// <summary>
    /// Identificador unico del dominio al que pertenece la unidad
    /// </summary>
    public string DominioId { get; set; }
}

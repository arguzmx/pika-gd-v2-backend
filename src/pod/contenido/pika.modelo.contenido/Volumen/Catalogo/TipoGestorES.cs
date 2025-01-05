using apigenerica.model.modelos;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;
using System.Xml.Serialization;

namespace pika.modelo.contenido;

/// <summary>
/// Catálogo para la disposición documental
/// </summary>
[ExcludeFromCodeCoverage]
public class TipoGestorES : ElementoCatalogo
{
    [XmlIgnore, JsonIgnore]
    public List<Volumen> Volumenes { get; set; } = new List<Volumen>();
}

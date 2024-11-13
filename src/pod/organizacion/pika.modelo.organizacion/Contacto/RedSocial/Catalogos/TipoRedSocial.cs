using apigenerica.model.modelos;

namespace pika.modelo.organizacion.Contacto;

/// <summary>
/// Tipos de redes sociales
/// </summary>
public class TipoRedSocial: ElementoCatalogo
{
    /// <summary>
    /// Propiedades de nevegacion
    /// </summary>
    public List<RedSocial>? RedesSociales { get; set; }

}

using Microsoft.Extensions.Logging;
using pika.comunes.almacenamiento.configuraciones;

namespace pika.comunes.almacenamiento.proveedores;

/// <summary>
/// Proveedor de almacenamiento local
/// </summary>
public class ProveedorAlmacenamientoBucketGCP : ProveedorAlmacenamientoBase
{
    private ConfiguracionBucketGCP _config;
    
    public ProveedorAlmacenamientoBucketGCP(ILogger logger, ConfiguracionBucketGCP config): 
        base(logger)
    {
        _config = config;
    }
   
}

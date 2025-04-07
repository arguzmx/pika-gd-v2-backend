using FluentStorage;
using Microsoft.Extensions.Logging;
using pika.comunes.almacenamiento.configuraciones;

namespace pika.comunes.almacenamiento.proveedores;

/// <summary>
/// Proveedor de almacenamiento local
/// </summary>
public class ProveedorAlmacenamientoFilesystemLocal : ProveedorAlmacenamientoBase
{
    private ConfiguracionFilesystemLocal _config;
    
    public ProveedorAlmacenamientoFilesystemLocal(ILogger logger, ConfiguracionFilesystemLocal config): 
        base(logger)
    {
        _config = config;
        _storage = StorageFactory.Blobs.DirectoryFiles(_config.Ruta);
    }


    
}

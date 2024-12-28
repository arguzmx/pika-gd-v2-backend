using comunes.primitivas;

namespace pika.servicios.contenido.Extensiones;

/// <summary>
/// Extensiones de resultado para las entidades
/// </summary>
public static class ExtensionesResultadoEntidad
{
    public static ErrorProceso ErrorBadRequest(this string propiedad, string? mensaje = null)
    {
        return new ErrorProceso
        {
            HttpCode = HttpCode.BadRequest,
            Codigo = "DATOSNOVALIDOS",
            Propiedad = propiedad,
            Mensaje = $"{propiedad} valor no válido {mensaje}".Trim()
        };
    }

    public static ErrorProceso ErrorNotFound(this string propiedad, string? mensaje = null)
    {
        return new ErrorProceso
        {
            HttpCode = HttpCode.NotFound,
            Codigo = "NOENCONTRADO",
            Propiedad = propiedad,
            Mensaje = $"{propiedad} {mensaje}".Trim()
        };
    }

    public static ErrorProceso ErrorConflict(this string propiedad, string? mensaje = null)
    {
        return new ErrorProceso
        {
            HttpCode = HttpCode.NotFound,
            Codigo = "CONFLICTO",
            Propiedad = propiedad,
            Mensaje = $"{propiedad} {mensaje}".Trim()
        };
    }
}

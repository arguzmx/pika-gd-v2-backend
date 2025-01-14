using apigenerica.primitivas;

namespace pika.api.contenido.Controllers;

public class EntidadGenericaN3Controller : ControladorGenericoN3
{
    private ILogger<EntidadGenericaN3Controller> _logger;

    public EntidadGenericaN3Controller(ILogger<EntidadGenericaN3Controller> logger, IHttpContextAccessor httpContextAccessor) : base(logger, httpContextAccessor)
    {
        _logger = logger;
    }
}

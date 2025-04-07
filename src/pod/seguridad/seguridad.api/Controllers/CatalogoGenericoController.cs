using apigenerica.primitivas;
using Microsoft.AspNetCore.Mvc;

namespace seguridad.api.Controllers;


[ApiController]
public class CatalogoGenericoController : ControladorCatalogoGenerico
{
    private ILogger<CatalogoGenericoController> _logger;

    public CatalogoGenericoController(ILogger<CatalogoGenericoController> logger, IHttpContextAccessor httpContextAccessor) : base (httpContextAccessor) 
    {
        _logger = logger;
    }
}

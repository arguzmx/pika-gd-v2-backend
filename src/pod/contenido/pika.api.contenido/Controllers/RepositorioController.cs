using apigenerica.primitivas;
using Microsoft.AspNetCore.Mvc;
using pika.modelo.contenido;
using pika.servicios.contenido.volumenrepositorio;
using System.Collections.Specialized;

namespace pika.api.contenido.Controllers;
[ApiController]
public class RepositorioController : ControladorBaseGenerico
{
    private readonly ILogger<RepositorioController> _logger;
    private readonly IServicioVolumenRepositorio _servicioVolumenRepositorio;
    private readonly IHttpContextAccessor _httpContextAccessor;
    /// <summary>
    /// Parametros de la petición que vienen desde el Middleware
    /// </summary>
    private StringDictionary _parametros;

    public RepositorioController(ILogger<RepositorioController> logger,IHttpContextAccessor httpContextAccessor, IServicioVolumenRepositorio servicioVolumenRepositorio) : base(httpContextAccessor)
    {
        _logger = logger;
        _httpContextAccessor = httpContextAccessor;
        _servicioVolumenRepositorio = servicioVolumenRepositorio;
        _parametros = this._servicioVolumenRepositorio.ParametrosRuta(_httpContextAccessor.HttpContext!);
    }

    [HttpPost("/entidad/repositorio/{repoId}/volumenrepositorio/{VolumentId}")]
    public async Task<ActionResult<VolumenDespliegue>> CreaAsociacion([FromBody] VolumenRepositorioCrear VolumenRepositorio)
    {
        _logger.LogDebug("RepositorioController - CreaAsociacion - {VolumenRepositorio}", VolumenRepositorio);
        
        var creaAsociacion = await this._servicioVolumenRepositorio.Insertar(VolumenRepositorio, _parametros);

        if(creaAsociacion.Ok)
        {
            return Ok(creaAsociacion.Payload);
        }

        return StatusCode(creaAsociacion.HttpCode.GetHashCode(), creaAsociacion.Error);
    }

    [HttpPut("/entidad/repositorio/{repoId}/volumenrepositorio/{VolumentId}")]
    public async Task<ActionResult> ActualizaAsociacion(string VolumentId, [FromBody] VolumenRepositorioActualizar VolumenRepositorio)
    {
        _logger.LogDebug("RepositorioController - ActualizaAsociacion {VolumenRepositorio}", VolumenRepositorio);

        var actualizaAsociacion = await this._servicioVolumenRepositorio.Actualizar(VolumentId, VolumenRepositorio, _parametros);
        if (actualizaAsociacion.Ok)
        {
            return Ok();
        }

        return StatusCode(actualizaAsociacion.HttpCode.GetHashCode(), actualizaAsociacion.Error);
    }

    [HttpDelete("/entidad/repositorio/{repoId}/volumenrepositorio/{VolumentId}")]
    public async Task<ActionResult> EliminaAsociacion(string VolumentId)
    {
        _logger.LogDebug("RepositorioController - EliminaAsociacion");
        var eliminaAsociacion = await this._servicioVolumenRepositorio.Eliminar(VolumentId, _parametros, true);
        if (eliminaAsociacion.Ok)
        {
            return Ok();
        }
        return StatusCode(eliminaAsociacion.HttpCode.GetHashCode(), eliminaAsociacion.Error);
    }

    [HttpGet("/entidad/repositorio/{repoId}/volumenrepositorio/pagina")]
    public async Task<ActionResult> PaginaVolumenes(string repoId)
    {
        _logger.LogDebug("PaginaVolumenes");
        var paginadoAsociacion = await this._servicioVolumenRepositorio.PaginaVolumenes(repoId);
        if (paginadoAsociacion.Ok)
        {
            return Ok(paginadoAsociacion.Payload);
        }
        return StatusCode(paginadoAsociacion.HttpCode.GetHashCode(), paginadoAsociacion.Error);
    }


}

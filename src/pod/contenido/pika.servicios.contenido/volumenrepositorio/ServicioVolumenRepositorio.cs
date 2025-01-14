using apigenerica.model.interpretes;
using apigenerica.model.modelos;
using apigenerica.model.reflectores;
using apigenerica.model.servicios;
using comunes.primitivas;
using CouchDB.Driver.Extensions;
using extensibilidad.metadatos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Serialization;
using pika.modelo.contenido;
using pika.servicios.contenido.dbcontext;
using System.Collections.Specialized;
using System.Net.WebSockets;
using System.Text.Json;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace pika.servicios.contenido.volumenrepositorio;
#pragma warning disable CS8603 // Possible null reference return.
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
[ServicioEntidadAPI(typeof(VolumenRepositorio))]
public class ServicioVolumenRepositorio : ServicioEntidadGenericaBase<VolumenRepositorio, VolumenRepositorioCrear, VolumenRepositorioActualizar, VolumenRepositorio, string>,
    IServicioVolumenRepositorio
{
    private DbContextContenido _localcontext;
    private Volumen _volumen;
    private IHttpContextAccessor _httpContextAccessor;
    private const string _DOMINIOHEADER = "x-d-id";
    private const string _UORGHEADER = "x-uo-id";
    public ServicioVolumenRepositorio(DbContextContenido context, ILogger<ServicioVolumenRepositorio> logger, IReflectorEntidadesAPI reflector, IDistributedCache cache, IHttpContextAccessor httpContextAccessor) : base(context, context.VolumenesRepositorio, logger, reflector, cache)
    {
        interpreteConsulta = new InterpreteConsultaMySQL();
        _localcontext = context;
        _httpContextAccessor = httpContextAccessor;
    }


    public DbContextContenido DB { get { return (DbContextContenido)_db; } }

    public bool RequiereAutenticacion => true;

    #region overrides para la entidad VolumenRepositorio

    public override async Task<ResultadoValidacion> ValidarInsertar(VolumenRepositorioCrear data)
    {
        _volumen = await DB.Volumenes.FindAsync(data.VolumenId);

        if (_volumen != null)
        {

            if ((_volumen.DominioId == _httpContextAccessor.HttpContext.Request.Headers[_DOMINIOHEADER]) && (_volumen.UOrgId == _httpContextAccessor.HttpContext.Request.Headers[_UORGHEADER]))
                return new ResultadoValidacion() { Valido = true};

            return new ResultadoValidacion()
            {
                Valido = false,
                Error = new ErrorProceso()
                {
                    Mensaje = "El Volumen no pertenece al mismo Dominio ó UnidadOrganizacional",
                    HttpCode = HttpCode.NotFound,
                    Codigo = CodigosError.CONTENIDO_VOLUMENREPOSITORIO_VOLUMEN_NO_ENCONTRADO
                }
            };
        }

        return new ResultadoValidacion()
        {
            Valido = false,
            Error = new ErrorProceso()
            {
                Mensaje = "No existe la entidad Volumen con el Id proporcionado",
                HttpCode = HttpCode.NotFound,
                Codigo = CodigosError.CONTENIDO_VOLUMEN_NO_ENCONTRADO
            }
        };
    }

    public override VolumenRepositorio ADTOFull(VolumenRepositorioCrear data)
    {
        return new VolumenRepositorio()
        {
            Id = Guid.NewGuid().ToString(),
            RepositorioId = data.RepositorioId,
            VolumenId = data.VolumenId,
            Default = data.Default,
            Activo = data.Activo
        };
    }

    public async Task<RespuestaPayload<VolumenDespliegue>> Insertar(VolumenRepositorioCrear data, StringDictionary? parametros = null)
    {
        RespuestaPayload<VolumenDespliegue> respuesta = new RespuestaPayload<VolumenDespliegue>();
        try
        {
            ResultadoValidacion resultadoValidacion = await ValidarInsertar(data);
            if (resultadoValidacion.Valido)
            {
                VolumenRepositorio entidad = ADTOFull(data);

                var existeVolumenRepo = _dbSetFull.Any(e => e.RepositorioId == parametros["repoId"] && e.VolumenId == parametros["VolumentId"]);

                if(existeVolumenRepo)
                {
                    respuesta.Ok = true;
                    return respuesta;
                }

                _dbSetFull.Add(entidad);
                await _db.SaveChangesAsync();
                respuesta.Ok = true;
                respuesta.HttpCode = HttpCode.Ok;
                respuesta.Payload = new VolumenDespliegue()
                {
                    Id = _volumen.Id,
                    Nombre = _volumen.Nombre
                };
            }
            else
            {
                respuesta.Error = resultadoValidacion.Error;
                respuesta.Error!.Codigo = resultadoValidacion.Error!.Codigo;
                respuesta.HttpCode = resultadoValidacion.Error?.HttpCode ?? HttpCode.None;
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "ServicioVolumenRepositorio-Insertar {msg}", ex.Message);
            respuesta.Error = new ErrorProceso() { Codigo = CodigosError.CONTENIDO_ERROR_DESCONOCIDO, HttpCode = HttpCode.ServerError, Mensaje = ex.Message };
            respuesta.HttpCode = HttpCode.ServerError;
        }
        return respuesta;
    }

    public override async Task<ResultadoValidacion> ValidarActualizar(string id, VolumenRepositorioActualizar actualizacion, VolumenRepositorio original)
    {
       _volumen = await DB.Volumenes.FindAsync(original.VolumenId);

        if (_volumen != null)
        {

            if ((_volumen.DominioId == _httpContextAccessor.HttpContext.Request.Headers[_DOMINIOHEADER]) && (_volumen.UOrgId == _httpContextAccessor.HttpContext.Request.Headers[_UORGHEADER]))
                return new ResultadoValidacion() { Valido = true };

            return new ResultadoValidacion()
            {
                Valido = false,
                Error = new ErrorProceso()
                {
                    Mensaje = "El Volumen no pertenece al mismo Dominio ó UnidadOrganizacional",
                    HttpCode = HttpCode.NotFound,
                    Codigo = CodigosError.CONTENIDO_VOLUMENREPOSITORIO_VOLUMEN_NO_ENCONTRADO
                }
            };
        }

        return new ResultadoValidacion()
        {
            Valido = false,
            Error = new ErrorProceso()
            {
                Mensaje = "No existe la entidad Volumen con el Id proporcionado",
                HttpCode = HttpCode.NotFound,
                Codigo = CodigosError.CONTENIDO_VOLUMEN_NO_ENCONTRADO
            }
        };
    }

    public override VolumenRepositorio ADTOFull(VolumenRepositorioActualizar actualizacion, VolumenRepositorio actual)
    {
        actual.Default = actualizacion.Default;
        actual.Activo = actualizacion.Activo;
        return actual;
    }



    public override async Task<Respuesta> Actualizar(string id, VolumenRepositorioActualizar data, StringDictionary? parametros = null)
    {
        Respuesta respuesta = new Respuesta();
        try
        {
            if (string.IsNullOrEmpty(id) || data == null)
            {
                respuesta.Error = new ErrorProceso()
                {
                    Codigo = CodigosError.CONTENIDO_VOLUMENREPOSITORIO_ID_PAYLOAD_NO_INGRESADO,
                    Mensaje = "No ha sido proporcionado el Id ó Payload",
                    HttpCode = HttpCode.BadRequest,
                };  
                respuesta.HttpCode = HttpCode.BadRequest;
                return respuesta;
            }

            VolumenRepositorio actual = _dbSetFull.FirstOrDefault(e => e.RepositorioId == parametros["repoId"] && e.VolumenId == parametros["VolumentId"]);

            if (actual == null)
            {
                respuesta.Error = new ErrorProceso()
                {
                    Codigo = CodigosError.CONTENIDO_VOLUMENREPOSITORIO_NO_ENCONTRADO,
                    Mensaje = "No se ha encontrado el VolumenRepositorio",
                    HttpCode = HttpCode.NotFound,
                };
                respuesta.HttpCode = HttpCode.NotFound;
                return respuesta;
            }

            ResultadoValidacion resultadoValidacion = await ValidarActualizar(id, data, actual);
            if (resultadoValidacion.Valido)
            {
                VolumenRepositorio entidad = ADTOFull(data, actual);
                _dbSetFull.Update(entidad);
                await _db.SaveChangesAsync();
                respuesta.Ok = true;
                respuesta.HttpCode = HttpCode.Ok;
            }
            else
            {
                respuesta.Error = resultadoValidacion.Error;
                respuesta.Error!.Codigo = resultadoValidacion.Error!.Codigo;
                respuesta.HttpCode = resultadoValidacion.Error?.HttpCode ?? HttpCode.None;
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "ServicioVolumenRepositorio-Actualizar {msg}", ex.Message);
            respuesta.Error = new ErrorProceso() { Codigo = CodigosError.CONTENIDO_ERROR_DESCONOCIDO, HttpCode = HttpCode.ServerError, Mensaje = ex.Message };
            respuesta.HttpCode = HttpCode.ServerError;
        }

        return respuesta;
    }

    public override async Task<ResultadoValidacion> ValidarEliminacion(string id, VolumenRepositorio original, bool forzarEliminacion = false)
    {
        _volumen = await DB.Volumenes.FindAsync(original.VolumenId);

        if (_volumen != null)
        {
            if ((_volumen.DominioId == _httpContextAccessor.HttpContext.Request.Headers[_DOMINIOHEADER]) && (_volumen.UOrgId == _httpContextAccessor.HttpContext.Request.Headers[_UORGHEADER]))
                return new ResultadoValidacion() { Valido = true };

            return new ResultadoValidacion()
            {
                Valido = false,
                Error = new ErrorProceso()
                {
                    Mensaje = "El Volumen no pertenece al mismo Dominio ó UnidadOrganizacional",
                    HttpCode = HttpCode.NotFound,
                    Codigo = CodigosError.CONTENIDO_VOLUMENREPOSITORIO_VOLUMEN_NO_ENCONTRADO
                }
            };
        }

        return new ResultadoValidacion()
        {
            Valido = false,
            Error = new ErrorProceso()
            {
                Mensaje = "No existe la entidad Volumen con el Id proporcionado",
                HttpCode = HttpCode.NotFound,
                Codigo = CodigosError.CONTENIDO_VOLUMEN_NO_ENCONTRADO
            }
        };
    }

    public override async Task<Respuesta> Eliminar(string id, StringDictionary? parametros = null, bool forzarEliminacion = false)
    {
        Respuesta respuesta = new Respuesta();
        try
        {

            VolumenRepositorio actual = _dbSetFull.FirstOrDefault(e => e.RepositorioId == parametros["repoId"] && e.VolumenId == parametros["VolumentId"]);

            if (actual == null)
            {
                respuesta.Ok = false;
                respuesta.Error = new ErrorProceso()
                {
                    Codigo = CodigosError.CONTENIDO_VOLUMENREPOSITORIO_NO_ENCONTRADO,
                    Mensaje = "No se ha encontrado el VolumenRepositorio",
                    HttpCode = HttpCode.NotFound,
                };
                respuesta.HttpCode = HttpCode.NotFound;
                return respuesta;
            }

            ResultadoValidacion resultadoValidacion = await ValidarEliminacion(id, actual, forzarEliminacion);
            if (resultadoValidacion.Valido)
            {
                var existeContenido = await DB.Contenidos.FirstOrDefaultAsync(e => e.VolumenId == parametros["VolumentId"] && e.RepositorioId == parametros["repoId"]);

                if (existeContenido != null)
                {
                    respuesta.Ok = false;
                    respuesta.Error = new ErrorProceso()
                    {
                        Mensaje = "No se puede eliminar ya que existe un Contenido Relacionado con el mismo Repositori y el Mismo Volumen",
                        HttpCode = HttpCode.NotFound,
                        Codigo = CodigosError.CONTENIDO_VOLUMENREPOSITORIO_ERROR_ELIMINAR
                    };
                    return respuesta;
                }

                _dbSetFull.Remove(actual);
                await _db.SaveChangesAsync();
                respuesta.Ok = true;
                respuesta.HttpCode = HttpCode.Ok;
            }
            else
            {
                respuesta.Error = resultadoValidacion.Error;
                respuesta.Error!.Codigo = resultadoValidacion.Error!.Codigo;
                respuesta.HttpCode = resultadoValidacion.Error?.HttpCode ?? HttpCode.None;
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "ServicioVolumenRepositorio-Eliminar {msg}", ex.Message);
            respuesta.Error = new ErrorProceso() { Codigo = CodigosError.CONTENIDO_ERROR_DESCONOCIDO, HttpCode = HttpCode.ServerError, Mensaje = ex.Message };
            respuesta.HttpCode = HttpCode.ServerError;
        }

        return respuesta;
    }

    public async Task<RespuestaPayload<List<Volumen>>> PaginaVolumenes(string repoId)
    {
        RespuestaPayload<List<Volumen>> respuesta = new();

        var volumenesId = _dbSetFull.Where(e => e.RepositorioId == repoId).Select(e => e.VolumenId).ToList();

        if (volumenesId.Count == 0)
        {
            respuesta.Ok = false;
            respuesta.Error = new ErrorProceso()
            {
                Mensaje = "No existen volumenes relacionados con el Id repositorio proporcionado",
                Codigo = CodigosError.CONTENIDO_VOLUMENREPOSITORIO_VOLUMENES_NO_ENCONTRADOS,
                HttpCode = HttpCode.Conflict
            };
            return respuesta;
        }
        List<Volumen> volumenes = new();
        foreach (var volumenId in volumenesId)
        {
            var volumen = await DB.Volumenes.FindAsync(volumenId);
            volumenes.Add(volumen!);
        }

        respuesta.Ok = true;
        respuesta.Payload = volumenes;
        return respuesta;
    }

    public StringDictionary ParametrosRuta(HttpContext context)
    {
        StringDictionary paramNiveles = new StringDictionary();

        var rutaDatos = context.GetRouteData().Values;

        var niveles = new[] { "repoId", "VolumentId" };

        foreach (var nivel in niveles)
        {
            if (rutaDatos.TryGetValue(nivel, out var valorRuta))
            {
                paramNiveles[nivel] = valorRuta?.ToString();
            }
            else
            {
                paramNiveles[nivel] = null;
            }
        }
        return paramNiveles;
    }

    #endregion
}
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.
#pragma warning restore CS8603 // Possible null reference return.
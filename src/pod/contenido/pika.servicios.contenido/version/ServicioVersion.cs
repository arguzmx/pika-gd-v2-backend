using apigenerica.model.interpretes;
using apigenerica.model.modelos;
using apigenerica.model.reflectores;
using apigenerica.model.servicios;
using comunes.primitivas;
using extensibilidad.metadatos;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using pika.modelo.contenido;
using pika.servicios.contenido.dbcontext;
using System.Collections.Specialized;
using System.Reflection.Metadata.Ecma335;
using System.Text.Json;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace pika.servicios.contenido.version;

//public class ServicioVersion : ServicioEntidadGenericaBase<EntidadVersion, VersionInsertar, VersionActualizar, VersionDespliegue, string>,
//    IServicioEntidadAPI, IServicioVersion
//{
//    private readonly ILogger<ServicioVersion> _logger;
//    private readonly VersionCouchDbContext dbCouch;
//    private readonly IServicioRepositorio _servicioRepositorio;
//    private readonly IServicioContenido _servicioContenido;
//    private readonly DbSet<EntidadVersion> _dbSetVersion;

//    public ServicioVersion(DbContext db, DbSet<EntidadVersion> dbSetVersion, ILogger<ServicioVersion> logger, VersionCouchDbContext dbCouch, IReflectorEntidadesAPI Reflector, IDistributedCache cache, IServicioRepositorio servicioRepositorio, IServicioContenido servicioContenido) : base(db, dbSetVersion, logger, Reflector, cache)
//    {
//        interpreteConsulta = new InterpreteConsultaMySQL();
//        this.dbCouch = dbCouch;
//        _servicioRepositorio = servicioRepositorio;
//        _servicioContenido = servicioContenido;
//        _dbSetVersion = dbSetVersion;
//    }

//    public bool RequiereAutenticacion => throw new NotImplementedException();

//    public Task<Respuesta> ActualizarAPI(object id, JsonElement data, StringDictionary? parametros = null)
//    {
//        throw new NotImplementedException();
//    }

//    public Task<Respuesta> EliminarAPI(object id, StringDictionary? parametros = null, bool forzarEliminacion = false)
//    {
//        throw new NotImplementedException();
//    }

//    public Entidad EntidadDespliegueAPI()
//    {
//        _logger.LogDebug("ServicioVersion - EntidadDespliegueAPI");
//        return this.EntidadDespliegue();
//    }

//    public Entidad EntidadInsertAPI()
//    {
//        _logger.LogDebug("ServicioVersion - EntidadInsertAPI");
//        return this.EntidadInsert();
//    }

//    public Entidad EntidadRepoAPI()
//    {
//        _logger.LogDebug("ServicioVersion - EntidadRepoAPI");
//        return this.EntidadRepo();
//    }

//    public Entidad EntidadUpdateAPI()
//    {
//        _logger.LogDebug("ServicioVersion - EntidadUpdateAPI");
//        return this.EntidadUpdate();
//    }

//    public void EstableceContextoUsuarioAPI(ContextoUsuario contexto)
//    {
//        _logger.LogDebug("ServicioVersion - EstableceContextoUsuarioAPI");
//        this.EstableceContextoUsuario(contexto);
//    }

//    public async Task<RespuestaPayload<object>> InsertarAPI(JsonElement data, StringDictionary? parametros = null)
//    {
//        _logger.LogDebug("ServicioVersion - InsertarAPI");
//        var add = data.Deserialize<VersionInsertar>(JsonAPIDefaults());
//        var temp = await this.Insertar(add, parametros);
//        RespuestaPayload<object> respuesta = JsonSerializer.Deserialize<RespuestaPayload<object>>(JsonSerializer.Serialize(temp));
//        return respuesta;
//    }

//    public ContextoUsuario? ObtieneContextoUsuarioAPI()
//    {
//        _logger.LogDebug("ServicioVersion - ObtieneContextoUsuarioAPI");
//        return this._contextoUsuario;
//    }

//    public async Task<RespuestaPayload<PaginaGenerica<object>>> PaginaAPI(Consulta consulta, StringDictionary? parametros = null)
//    {
//        _logger.LogDebug("ServicioVerision - PaginaAPI");
//        var temp = await this.Pagina(consulta, parametros);
//        RespuestaPayload<PaginaGenerica<object>> respuesta = JsonSerializer.Deserialize<RespuestaPayload<PaginaGenerica<object>>> (JsonSerializer.Serialize(temp));
//        return respuesta;
//    }

//    public async Task<RespuestaPayload<PaginaGenerica<object>>> PaginaDespliegueAPI(Consulta consulta, StringDictionary? parametros = null)
//    {
//        _logger.LogDebug("ServicioVersion - PaginaDespliegueAPI");
//        var temp = await this.PaginaDespliegue(consulta, parametros);
//        RespuestaPayload<PaginaGenerica<object>> respuesta = JsonSerializer.Deserialize<RespuestaPayload<PaginaGenerica<object>>> (JsonSerializer.Serialize(temp));
//        return respuesta;
//    }

//    public async Task<RespuestaPayload<object>> UnicaPorIdAPI(object id, StringDictionary? parametros = null)
//    {
//        _logger.LogDebug("ServicioVersion - UnicaPorIdAPI");
//        var temp = await this.UnicaPorId((string)id, parametros);
//        RespuestaPayload<object> respuesta = JsonSerializer.Deserialize<RespuestaPayload<object>>(JsonSerializer.Serialize(temp));
//        return respuesta;
//    }

//    public async Task<RespuestaPayload<object>> UnicaPorIdDespliegueAPI(object id, StringDictionary? parametros = null)
//    {
//        _logger.LogDebug("ServicioVersion - UnicaPorIdDespliegue");
//        var temp = await this.UnicaPorIdDespliegue((string)id, parametros);
//        RespuestaPayload<object> respuesta = JsonSerializer.Deserialize<RespuestaPayload<object>>(JsonSerializer.Serialize(temp));
//        return respuesta;
//    }

//    #region Overrides de la Entidad Version [N3]

//    public override async Task<ResultadoValidacion> ValidarInsertar(VersionInsertar data)
//    {
//        var resultado = new ResultadoValidacion();

//        var repo = await this._servicioRepositorio.UnicaPorId(data.RepositorioId);
//        if(repo.Payload != null)
//        {
//            var existeContenido = await this._servicioContenido.UnicaPorId(data.ContenidoId);
//            if(existeContenido.Payload != null)
//            {
//                var contenido = (Contenido)existeContenido.Payload;
//                if (contenido.RepositorioId == data.RepositorioId)
//                {
//                    resultado.Valido = true;
//                }
//                else
//                {
//                    resultado.Error = new ErrorProceso()
//                    {
//                        Mensaje = "El contenido no pertenece al Repositorio",
//                        Codigo = CodigosError.CONTENIDO_VERSION_ERROR_CONTENIDO,
//                        HttpCode = HttpCode.Conflict
//                    };
//                }
//            }
//            else
//            {
//                resultado.Error = new ErrorProceso()
//                {
//                    Mensaje = "El contenido no ha sido encontrado con el Id proporcionado en el Payload",
//                    Codigo = CodigosError.CONTENIDO_VERSION_ERROR_CONTENIDO_NO_ENCONTRADO,
//                    HttpCode = HttpCode.NotFound
//                };
//            }
//        }
//        else
//        {
//            resultado.Error = new ErrorProceso()
//            {
//                Mensaje = "El repositorio no existe",
//                Codigo = CodigosError.CONTENIDO_VERSION_ERROR_CONTENIDO_NO_ENCONTRADO,
//                HttpCode = HttpCode.Conflict
//            };
//        }
//        return resultado;
//    }

//    public override EntidadVersion ADTOFull(VersionInsertar data)
//    {
//        return new EntidadVersion()
//        {
//            Id = Guid.NewGuid().ToString(),
//            RepositorioId = data.RepositorioId,
//            ContenidoId = data.ContenidoId,
//            Activa = data.Activa,
//            VolumenId = data.VolumenId
//        };
//    }

//    public override async Task<RespuestaPayload<VersionDespliegue>> Insertar(VersionInsertar data, StringDictionary? parametros = null)
//    {
//        RespuestaPayload<VersionDespliegue> respuesta = new RespuestaPayload<VersionDespliegue>();
//        try
//        {
//            ResultadoValidacion resultadoValidacion = await ValidarInsertar(data);
//            if (resultadoValidacion.Valido)
//            {
//                EntidadVersion entidad = ADTOFull(data);
//                _dbSetFull.Add(entidad);
//                await _db.SaveChangesAsync();
//                respuesta.Ok = true;
//                respuesta.HttpCode = HttpCode.Ok;
//                respuesta.Payload = ADTODespliegue(entidad);
//            }
//            else
//            { 
//                respuesta.Error = resultadoValidacion.Error;
//                respuesta.Error!.Codigo = resultadoValidacion.Error!.Codigo;
//                respuesta.HttpCode = resultadoValidacion.Error?.HttpCode ?? HttpCode.None;
//            }
//        }
//        catch (Exception ex)
//        {
//            _logger.LogError(ex, "ServicioVersion-Insertar {msg}", ex.Message);
//            respuesta.Error = new ErrorProceso() { Codigo = CodigosError.CONTENIDO_ERROR_DESCONOCIDO, HttpCode = HttpCode.ServerError, Mensaje = ex.Message };
//            respuesta.HttpCode = HttpCode.ServerError;
//        }

//        return respuesta;
//    }

//    public override async  Task<ResultadoValidacion> ValidarActualizar(string id, VersionActualizar actualizacion, EntidadVersion original)
//    {
//        var resultado = new ResultadoValidacion();

//        var repo = await this._servicioRepositorio.UnicaPorId(original.RepositorioId);
//        if (repo.Payload != null)
//        {
//            var existeContenido = await this._servicioContenido.UnicaPorId(original.ContenidoId);
//            if (existeContenido.Payload != null)
//            {
//                var contenido = (Contenido)existeContenido.Payload;
//                if (contenido.RepositorioId == original.RepositorioId)
//                {
//                    resultado.Valido = true;
//                }
//                else
//                {
//                    resultado.Error = new ErrorProceso()
//                    {
//                        Mensaje = "El contenido no pertenece al Repositorio",
//                        Codigo = CodigosError.CONTENIDO_VERSION_ERROR_CONTENIDO,
//                        HttpCode = HttpCode.Conflict
//                    };
//                }
//            }
//            else
//            {
//                resultado.Error = new ErrorProceso()
//                {
//                    Mensaje = "El contenido no ha sido encontrado con el Id proporcionado en el Payload",
//                    Codigo = CodigosError.CONTENIDO_VERSION_ERROR_CONTENIDO_NO_ENCONTRADO,
//                    HttpCode = HttpCode.NotFound
//                };
//            }
//        }
//        else
//        {
//            resultado.Error = new ErrorProceso()
//            {
//                Mensaje = "El repositorio no existe",
//                Codigo = CodigosError.CONTENIDO_VERSION_ERROR_CONTENIDO_NO_ENCONTRADO,
//                HttpCode = HttpCode.Conflict
//            };
//        }
//        return resultado;
//    }

//    public override EntidadVersion ADTOFull(VersionActualizar actualizacion, EntidadVersion actual)
//    {
//        actual.Id = actualizacion.Id;
//        actual.Activa = actualizacion.Activa;
//        return actual;
        
//    }
//    public override async Task<Respuesta> Actualizar(string id, VersionActualizar data, StringDictionary? parametros = null)
//    {
//        Respuesta respuesta = new Respuesta();
//        try
//        {
//            if (string.IsNullOrEmpty(id) || data == null)
//            {
//                respuesta.Error = new ErrorProceso()
//                {
//                    Codigo = CodigosError.CONTENIDO_VERSION_ID_PAYLOAD_NO_INGRESADO,
//                    Mensaje = "No ha sido proporcionado el Id ó Payload",
//                    HttpCode = HttpCode.BadRequest,
//                };
//                respuesta.HttpCode = HttpCode.BadRequest;
//                return respuesta;
//            }

//            EntidadVersion actual = _dbSetFull.Find(id);
//            if (actual == null)
//            {
//                respuesta.Error = new ErrorProceso()
//                {
//                    Codigo = CodigosError.CONTENIDO_VERSION_NO_ENCONTRADO,
//                    Mensaje = "No se ha encontrado el VolumenRepositorio",
//                    HttpCode = HttpCode.NotFound,
//                };
//                respuesta.HttpCode = HttpCode.NotFound;
//                return respuesta;
//            }

//            ResultadoValidacion resultadoValidacion = await ValidarActualizar(id, data, actual);
//            if (resultadoValidacion.Valido)
//            {
//                EntidadVersion entidad = ADTOFull(data, actual);
//                _dbSetFull.Update(entidad);
//                await _db.SaveChangesAsync();
//                respuesta.Ok = true;
//                respuesta.HttpCode = HttpCode.Ok;
//            }
//            else
//            {
//                respuesta.Error = resultadoValidacion.Error;
//                respuesta.Error!.Codigo = resultadoValidacion.Error!.Codigo;
//                respuesta.HttpCode = resultadoValidacion.Error?.HttpCode ?? HttpCode.None;
//            }
//        }
//        catch (Exception ex)
//        {
//            _logger.LogError(ex, "ServicioVersion-Actualizar {msg}", ex.Message);
//            respuesta.Error = new ErrorProceso() { Codigo = CodigosError.CONTENIDO_ERROR_DESCONOCIDO, HttpCode = HttpCode.ServerError, Mensaje = ex.Message };
//            respuesta.HttpCode = HttpCode.ServerError;
//        }

//        return respuesta;
//    }

//    public override async Task<ResultadoValidacion> ValidarEliminacion(string id, EntidadVersion original, bool forzarEliminacion = false)
//    {
//        var resultado = new ResultadoValidacion();

//        var repo = await this._servicioRepositorio.UnicaPorId(original.RepositorioId);
//        if (repo.Payload != null)
//        {
//            var existeContenido = await this._servicioContenido.UnicaPorId(original.ContenidoId);
//            if (existeContenido.Payload != null)
//            {
//                var contenido = (Contenido)existeContenido.Payload;
//                if (contenido.RepositorioId == original.RepositorioId)
//                {
//                    resultado.Valido = true;
//                }
//                else
//                {
//                    resultado.Error = new ErrorProceso()
//                    {
//                        Mensaje = "El contenido no pertenece al Repositorio",
//                        Codigo = CodigosError.CONTENIDO_VERSION_ERROR_CONTENIDO,
//                        HttpCode = HttpCode.Conflict
//                    };
//                }
//            }
//            else
//            {
//                resultado.Error = new ErrorProceso()
//                {
//                    Mensaje = "El contenido no ha sido encontrado con el Id proporcionado en el Payload",
//                    Codigo = CodigosError.CONTENIDO_VERSION_ERROR_CONTENIDO_NO_ENCONTRADO,
//                    HttpCode = HttpCode.NotFound
//                };
//            }
//        }
//        else
//        {
//            resultado.Error = new ErrorProceso()
//            {
//                Mensaje = "El repositorio no existe",
//                Codigo = CodigosError.CONTENIDO_VERSION_ERROR_CONTENIDO_NO_ENCONTRADO,
//                HttpCode = HttpCode.Conflict
//            };
//        }
//        return resultado;
//    }


//    public override async Task<Respuesta> Eliminar(string id, StringDictionary? parametros = null, bool forzarEliminacion = false)
//    {
//        Respuesta respuesta = new Respuesta();
//        try
//        {
//            if (string.IsNullOrEmpty(id))
//            {
//                respuesta.Error = new ErrorProceso()
//                {
//                    Codigo = CodigosError.CONTENIDO_VERSION_ID_NO_INGRESADO,
//                    Mensaje = "No ha sido proporcionado el Id",
//                    HttpCode = HttpCode.BadRequest
//                };
//                respuesta.HttpCode = HttpCode.BadRequest;
//                return respuesta;
//            }

//            EntidadVersion actual = _dbSetFull.Find(id);

//            if (actual == null)
//            {
//                respuesta.Error = new ErrorProceso()
//                {
//                    Codigo = CodigosError.CONTENIDO_VERSION_NO_ENCONTRADO,
//                    Mensaje = "No existe un Miembro con el Id proporcionado",
//                    HttpCode = HttpCode.NotFound
//                };
//                respuesta.HttpCode = HttpCode.NotFound;
//                return respuesta;
//            }

//            ResultadoValidacion resultadoValidacion = await ValidarEliminacion(id, actual, forzarEliminacion);
//            if (resultadoValidacion.Valido)
//            {
//                _dbSetFull.Remove(actual);
//                await _db.SaveChangesAsync();
//                respuesta.Ok = true;
//                respuesta.HttpCode = HttpCode.Ok;
//            }
//            else
//            {
//                respuesta.Error = resultadoValidacion.Error;
//                respuesta.HttpCode = resultadoValidacion.Error?.HttpCode ?? HttpCode.None;
//            }
//        }
//        catch (Exception ex)
//        {
//            _logger.LogError(ex, "ServicioVersion-Eliminar {msg}", ex.Message);
//            respuesta.Error = new ErrorProceso() { Codigo = CodigosError.CONTENIDO_ERROR_DESCONOCIDO, HttpCode = HttpCode.ServerError, Mensaje = ex.Message };
//            respuesta.HttpCode = HttpCode.ServerError;
//        }

//        return respuesta;
//    }


//    #endregion

//}

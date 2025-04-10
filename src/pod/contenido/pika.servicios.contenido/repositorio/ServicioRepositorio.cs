using apigenerica.model.abstracciones;
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
using System.Text.Json;

namespace pika.servicios.contenido.repositorio;

[ServicioEntidadAPI(entidad: typeof(Repositorio))]
public class ServicioRepositorio : ServicioEntidadGenericaBase<Repositorio, RepositorioInsertar, RepositorioActualizar, RepositorioDespliegue, string>,
    IServicioEntidadAPI, IServicioRepositorio
{
    private DbContextContenido localContext;

    public ServicioRepositorio(DbContextContenido context, ILogger<ServicioRepositorio> logger, IReflectorEntidadesAPI Reflector, IDistributedCache cache) : base(context, context.Repositorios, logger, Reflector, cache)
    {
        interpreteConsulta = new InterpreteConsultaMySQL();
        localContext = context;
    }


    /// <summary>
    /// Acceso al repositorio de gestipon documental local
    /// </summary>
    private DbContextContenido DB { get { return (DbContextContenido)_db; } }

    public bool RequiereAutenticacion => true;


    public async Task<Respuesta> ActualizarAPI(object id, JsonElement data, StringDictionary? parametros = null)
    {
        var update = data.Deserialize<RepositorioActualizar>(JsonAPIDefaults());
        return await this.Actualizar((string)id, update, parametros);
    }

    public async Task<Respuesta> EliminarAPI(object id, StringDictionary? parametros = null, bool forzarEliminacion = false)
    {
        return await this.Eliminar((string)id, parametros, forzarEliminacion);
    }

    public Entidad EntidadDespliegueAPI()
    {
        return this.EntidadDespliegue();
    }

    public Entidad EntidadInsertAPI()
    {
        return this.EntidadInsert();
    }

    public Entidad EntidadRepoAPI()
    {
        return this.EntidadRepo();
    }

    public Entidad EntidadUpdateAPI()
    {
        return this.EntidadUpdate();
    }

    public void EstableceContextoUsuarioAPI(ContextoUsuario contexto)
    {
        this.EstableceContextoUsuario(contexto);
    }

    public ContextoUsuario? ObtieneContextoUsuarioAPI()
    {
        return this._contextoUsuario;
    }

    public async Task<RespuestaPayload<object>> InsertarAPI(JsonElement data, StringDictionary? parametros = null)
    {
        var add = data.Deserialize<RepositorioInsertar>(JsonAPIDefaults());
        var temp = await this.Insertar(add, parametros);
        return temp.ReserializePayloadCamelCase(); ;
    }

    public async Task<RespuestaPayload<PaginaGenerica<object>>> PaginaAPI(Consulta consulta, StringDictionary? parametros = null)
    {
        var temp = await this.Pagina(consulta, parametros);
        return temp.ReserializePaginaCamelCase();
    }

    public async Task<RespuestaPayload<PaginaGenerica<object>>> PaginaDespliegueAPI(Consulta consulta, StringDictionary? parametros = null)
    {
        var temp = await this.PaginaDespliegue(consulta, parametros);
        return temp.ReserializePaginaCamelCase();
    }


    public async Task<RespuestaPayload<object>> UnicaPorIdAPI(object id, StringDictionary? parametros = null)
    {
        var temp = await this.UnicaPorId((string)id, parametros);
        return temp.ReserializePayloadCamelCase(); ;
    }

    public async Task<RespuestaPayload<object>> UnicaPorIdDespliegueAPI(object id, StringDictionary? parametros = null)
    {
        var temp = await this.UnicaPorIdDespliegue((string)id, parametros);
        return temp.ReserializePayloadCamelCase();
    }

    //#region Arbol

    //public override async Task<RespuestaPayload<List<ParClaveTexto>>> Raices(string? contextoId)
    //{
    //    await Task.Delay(0);
    //    RespuestaPayload<List<ParClaveTexto>> respuesta = new();
    //    if (string.IsNullOrEmpty(contextoId))
    //    {
    //        respuesta.HttpCode = HttpCode.BadRequest;
    //        respuesta.Error = new ErrorProceso()
    //        {
    //            Codigo = "",
    //            HttpCode = HttpCode.BadRequest,
    //            Mensaje = "RepositorioId debe ser utilizado como contextoId"
    //        };
    //        return respuesta;
    //    }

    //    var raices = this.localContext.Carpetas.Where(c => c.EsRaiz == true && c.RepositorioId == contextoId).ToList();

    //    respuesta.Payload = raices.Select(c => new ParClaveTexto() { Clave = c.Id, Texto = c.Nombre }).ToList();
    //    respuesta.Ok = true;
    //    respuesta.HttpCode = HttpCode.Ok;

    //    return respuesta;
    //}

    //public override async Task<RespuestaPayload<List<ParClaveTexto>>> Hijos(string id, string? contextoId)
    //{
    //    await Task.Delay(0);
    //    RespuestaPayload<List<ParClaveTexto>> respuesta = new();
    //    if (string.IsNullOrEmpty(contextoId))
    //    {
    //        respuesta.HttpCode = HttpCode.BadRequest;
    //        respuesta.Error = new ErrorProceso()
    //        {
    //            Codigo = "",
    //            HttpCode = HttpCode.BadRequest,
    //            Mensaje = "RepositorioId debe ser utilizado como contextoId"
    //        };
    //        return respuesta;
    //    }

    //    var raices = this.localContext.Carpetas.Where(c => c.CarpetaPadreId == id && c.RepositorioId == contextoId).ToList();

    //    respuesta.Payload = raices.Select(c => new ParClaveTexto() { Clave = c.Id, Texto = c.Nombre }).ToList();
    //    respuesta.Ok = true;
    //    respuesta.HttpCode = HttpCode.Ok;

    //    return respuesta;
    //}

    //public override async Task<RespuestaPayload<List<ParClaveTextoNodoArbol<string>>>> Arbol(string? contextoId)
    //{
    //    await Task.Delay(0);
    //    RespuestaPayload<List<ParClaveTextoNodoArbol<string>>> respuesta = new();
    //    if (string.IsNullOrEmpty(contextoId))
    //    {
    //        respuesta.HttpCode = HttpCode.BadRequest;
    //        respuesta.Error = new ErrorProceso()
    //        {
    //            Codigo = "",
    //            HttpCode = HttpCode.BadRequest,
    //            Mensaje = "RepositorioId debe ser utilizado como contextoId"
    //        };
    //        return respuesta;
    //    }

    //    var raices = this.localContext.Carpetas.Where(c => c.RepositorioId == contextoId).ToList();

    //    respuesta.Payload = raices.Select(c => new ParClaveTextoNodoArbol<string>() { PadreId = c.CarpetaPadreId, Clave = c.Id, Texto = c.Nombre }).ToList();
    //    respuesta.Ok = true;
    //    respuesta.HttpCode = HttpCode.Ok;

    //    return respuesta;
    //}

    //#endregion

    #region Overrides para la personalización de la entidad Repositorio

    public override async Task<ResultadoValidacion> ValidarInsertar(RepositorioInsertar data)
    {
        ResultadoValidacion resultado = new();
        bool encontrado = await DB.Repositorios.AnyAsync(a => a.UOrgId == _contextoUsuario!.UOrgId
                && a.DominioId == _contextoUsuario.DominioId
                && a.Nombre == data.Nombre);

        if (encontrado)
        {
            resultado.Error = "Nombre".ErrorProcesoDuplicado();
        }
        else
        {
            resultado.Valido = true;
        }

        return resultado;
    }


    public override async Task<ResultadoValidacion> ValidarEliminacion(string id, Repositorio original, bool forzarEliminacion = false)
    {
        ResultadoValidacion resultado = new();
        bool encontrado = await DB.Repositorios.AnyAsync(a => a.UOrgId == _contextoUsuario!.UOrgId
                && a.DominioId == _contextoUsuario.DominioId
                && a.Id == id);

        if (!encontrado)
        {

            resultado.Error = "id".ErrorProcesoNoEncontrado();

        }
        else
        {
            bool EncontradoCarpeta = await DB.Carpetas.AnyAsync(a => a.RepositorioId == id);
            bool EncontradoContenido = await DB.Contenidos.AnyAsync(a => a.RepositorioId == id);
            if (EncontradoCarpeta || EncontradoContenido)
            {
                resultado.Error = "Id en uso verifique que este no se encuentre en Carpeto O Contenido".Error409();
            }
            else
            {
                resultado.Valido = true;
            }
        }

        return resultado;
    }


    public override async Task<ResultadoValidacion> ValidarActualizar(string id, RepositorioActualizar actualizacion, Repositorio original)
    {
        ResultadoValidacion resultado = new();
        bool encontrado = await DB.Repositorios.AnyAsync(a => a.UOrgId == _contextoUsuario!.UOrgId
                && a.DominioId == _contextoUsuario.DominioId
                && a.Id == id);

        if (!encontrado)
        {
            resultado.Error = "id".ErrorProcesoNoEncontrado();

        }
        else
        {
            // Verifica que no haya un registro con el mismo nombre para el mismo dominio y UO en un resgitrso diferente
            bool duplicado = await DB.Repositorios.AnyAsync(a => a.UOrgId == _contextoUsuario!.UOrgId
                && a.DominioId == _contextoUsuario.DominioId
                && a.Id != id
                && a.Nombre.Equals(actualizacion.Nombre));

            if (duplicado)
            {
                resultado.Error = "Nombre".ErrorProcesoDuplicado();

            }
            else
            {
                resultado.Valido = true;
            }
        }

        return resultado;
    }


    public override Repositorio ADTOFull(RepositorioActualizar actualizacion, Repositorio actual)
    {
        actual.Nombre = actualizacion.Nombre;
        actual.VolumenId = actualizacion.VolumenId;
        return actual;
    }

    public override Repositorio ADTOFull(RepositorioInsertar data)
    {
        Repositorio repositorio = new()
        {
            Id = Guid.NewGuid().ToString(),
            UOrgId = _contextoUsuario!.UOrgId!,
            DominioId = _contextoUsuario!.DominioId!,
            Nombre = data.Nombre,
            VolumenId = data.VolumenId

        };
        return repositorio;
    }

    public override RepositorioDespliegue ADTODespliegue(Repositorio data)
    {
        RepositorioDespliegue repositorioDespliegue = new()
        {
            Id = data.Id,
            Nombre = data.Nombre,
            VolumenId = data.VolumenId
        };
        return repositorioDespliegue;
    }

    #endregion

}
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

#pragma warning disable CS8603 // Possible null reference return.
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
namespace pika.servicios.contenido;

[ServicioEntidadAPI(entidad: typeof(Carpeta))]
public class ServicioCarpeta : ServicioEntidadGenericaBase<Carpeta, CarpetaInsertar, CarpetaActualizar, CarpetaDespliegue, string>,
    IServicioEntidadAPI, IServicioCarpeta
{

    private DbContextContenido localContext;

    public ServicioCarpeta(DbContextContenido context, ILogger<ServicioCarpeta> logger, IReflectorEntidadesAPI Reflector, IDistributedCache cache) : base(context, context.Carpetas, logger, Reflector, cache)
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
        var update = data.Deserialize<CarpetaActualizar>(JsonAPIDefaults());
        return await this.Actualizar((string)id, update, parametros);
    }

    public async Task<Respuesta> EliminarAPI(object id, StringDictionary? parametros = null)
    {
        return await this.Eliminar((string)id, parametros);
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
        var add = data.Deserialize<CarpetaInsertar>(JsonAPIDefaults());
        var temp = await this.Insertar(add, parametros);
        RespuestaPayload<object> respuesta = JsonSerializer.Deserialize<RespuestaPayload<object>>(JsonSerializer.Serialize(temp));
        return respuesta;
    }

    public async Task<RespuestaPayload<PaginaGenerica<object>>> PaginaAPI(Consulta consulta, StringDictionary? parametros = null)
    {
        var temp = await this.Pagina(consulta, parametros);
        RespuestaPayload<PaginaGenerica<object>> respuesta = JsonSerializer.Deserialize<RespuestaPayload<PaginaGenerica<object>>>(JsonSerializer.Serialize(temp));
        return respuesta;
    }

    public async Task<RespuestaPayload<PaginaGenerica<object>>> PaginaDespliegueAPI(Consulta consulta, StringDictionary? parametros = null)
    {
        var temp = await this.PaginaDespliegue(consulta, parametros);
        RespuestaPayload<PaginaGenerica<object>> respuesta = JsonSerializer.Deserialize<RespuestaPayload<PaginaGenerica<object>>>(JsonSerializer.Serialize(temp));
        return respuesta;
    }

    public async Task<RespuestaPayload<object>> UnicaPorIdAPI(object id, StringDictionary? parametros = null)
    {
        var temp = await this.UnicaPorId((string)id, parametros);
        RespuestaPayload<object> respuesta = JsonSerializer.Deserialize<RespuestaPayload<object>>(JsonSerializer.Serialize(temp));
        return respuesta;
    }

    public async Task<RespuestaPayload<object>> UnicaPorIdDespliegueAPI(object id, StringDictionary? parametros = null)
    {
        var temp = await this.UnicaPorIdDespliegue((string)id, parametros);

        RespuestaPayload<object> respuesta = JsonSerializer.Deserialize<RespuestaPayload<object>>(JsonSerializer.Serialize(temp));
        return respuesta;
    }

    #region Overrides para la personalización de la entidad Carpeta

    public override async Task<ResultadoValidacion> ValidarInsertar(CarpetaInsertar data)
    {
        ResultadoValidacion resultado = new();
        bool encontrado = await DB.Carpetas.AnyAsync(a => a.Nombre == data.Nombre);

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


    //public override async Task<ResultadoValidacion> ValidarEliminacion(string id, Carpeta original)
    //{
    //    ResultadoValidacion resultado = new();
    //    bool encontrado = await DB.Carpetas.AnyAsync(a => a.Id == id);

    //    if (!encontrado)
    //    {

    //        resultado.Error = "id".ErrorProcesoNoEncontrado();

    //    }
    //    else
    //    {
    //        bool EncontradoContenido = await DB.Contenidos.AnyAsync(a => a.CarpetaId == id);
    //        if (EncontradoContenido)
    //        {
    //            resultado.Error = "Id en uso verifique que este no este en Contenido".Error409();
    //        }
    //        else
    //        {
    //            resultado.Valido = true;
    //        }
    //    }

    //    return resultado;
    //}


    public override async Task<ResultadoValidacion> ValidarActualizar(string id, CarpetaActualizar actualizacion, Carpeta original)
    {
        ResultadoValidacion resultado = new();

        bool duplicado = await DB.Carpetas.AnyAsync(a => a.Id != id && a.Nombre.Equals(actualizacion.Nombre));

        if (duplicado)
        {
            resultado.Error = "Nombre".ErrorProcesoDuplicado();

        }
        else
        {
            resultado.Valido = true;
        }


        return resultado;
    }


    public override Carpeta ADTOFull(CarpetaActualizar actualizacion, Carpeta actual)
    {
        actual.Nombre = actualizacion.Nombre;
        actual.CarpetaPadreId = actualizacion.CarpetaPadreId;
        return actual;
    }

    public override Carpeta ADTOFull(CarpetaInsertar data)
    {
        Carpeta carpeta = new()
        {
            Id = Guid.NewGuid().ToString(),
            RepositorioId = data.RepositorioId,
            CreadorId = _contextoUsuario!.UsuarioId!,
            FechaCreacion = DateTime.Now,
            Nombre = data.Nombre,
            CarpetaPadreId = data.CarpetaPadreId,
            EsRaiz = data.CarpetaPadreId == null,
            PermisoId = "OperacionEspecial"

        };
        return carpeta;
    }

    public override CarpetaDespliegue ADTODespliegue(Carpeta data)
    {
        CarpetaDespliegue carpetaDespliegue = new()
        {
            Id = data.Id,
            CreadorId = data.CreadorId,
            FechaCreacion = data.FechaCreacion,
            Nombre = data.Nombre,
            CarpetaPadreId = data.CarpetaPadreId
        };
        return carpetaDespliegue;
    }
    #endregion

}
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.
#pragma warning restore CS8603 // Possible null reference return.

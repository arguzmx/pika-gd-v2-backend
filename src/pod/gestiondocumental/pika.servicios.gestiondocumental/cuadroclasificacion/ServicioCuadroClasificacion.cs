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
using pika.modelo.gestiondocumental;
using System.Collections.Specialized;
using System.Text.Json;

namespace pika.servicios.gestiondocumental.cuadroclasificacion;


[ServicioEntidadAPI(entidad: typeof(CuadroClasificacion))]
public class ServicioCuadroClasificacion : ServicioEntidadGenericaBase<CuadroClasificacion, CuadroClasificacionInsertar, CuadroClasificacionActualizar, CuadroClasificacionDespliegue, string>,
    IServicioEntidadAPI, IServicioCuadroclasificacion
{

    private DbContextGestionDocumental localContext;

    public ServicioCuadroClasificacion(DbContextGestionDocumental context, ILogger<ServicioCuadroClasificacion> logger, IReflectorEntidadesAPI Reflector, IDistributedCache cache) : base(context, context.CuadrosClasificacion, logger, Reflector, cache)
    {
        interpreteConsulta = new InterpreteConsultaMySQL();
        localContext = context;
    }

    /// <summary>
    /// Acceso al repositorio de gestipon documental local
    /// </summary>
    private DbContextGestionDocumental DB { get { return (DbContextGestionDocumental)_db; } }


    public bool RequiereAutenticacion => true;

    public async Task<Respuesta> ActualizarAPI(object id, JsonElement data, StringDictionary? parametros = null)
    {
        var update = data.Deserialize<CuadroClasificacionActualizar>(JsonAPIDefaults());
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
        var add = data.Deserialize<CuadroClasificacionInsertar>(JsonAPIDefaults());
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

    #region Overrides para la personalizacion de la entidad cuadro clasificacion

    public override async Task<ResultadoValidacion> ValidarInsertar(CuadroClasificacionInsertar insertar)
    {

        ResultadoValidacion resultado = new();
        bool encontrado = await DB.CuadrosClasificacion.AnyAsync(a => a.UOrgId == _contextoUsuario!.UOrgId
                && a.DominioId == _contextoUsuario.DominioId
                && a.Nombre == insertar.Nombre);

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

    public override async Task<ResultadoValidacion> ValidarEliminacion(string id, CuadroClasificacion original)
    {
        ResultadoValidacion resultado = new();
        bool encontrado = await DB.CuadrosClasificacion.AnyAsync(a => a.UOrgId == _contextoUsuario!.UOrgId
                && a.DominioId == _contextoUsuario.DominioId
                && a.Id == id);

        if (!encontrado)
        {

            resultado.Error = "id".ErrorProcesoNoEncontrado();

        }
        else
        {
            //    bool EncontradoActivo = await DB.Activos.AnyAsync(a => a.CuadroClasificacionId == id);
            //    bool EncontradoSerieDocumental = await DB.SerieDocumentales.AnyAsync(a => a.CuadroClasificacionId == id);
            //    bool EncontradoTransferencia = await DB.Transferencias.AnyAsync(a => a.CuadroClasificacionId == id);
            //    if (EncontradoActivo || EncontradoSerieDocumental || EncontradoTransferencia)
            //    {
            //        resultado.Error = "Id en uso verifique que este no se encuentre en Activos,SerieDocumental O Transferencia".Error409();
            //    }
            //    else
            //    {
            //        resultado.Valido = true;
            //    }
            resultado.Valido = true;
        }

        return resultado;
    }

    public override async Task<ResultadoValidacion> ValidarActualizar(string id, CuadroClasificacionActualizar actualizar, CuadroClasificacion original)
    {
        ResultadoValidacion resultado = new();
        bool encontrado = await DB.CuadrosClasificacion.AnyAsync(a => a.UOrgId == _contextoUsuario!.UOrgId
                && a.DominioId == _contextoUsuario.DominioId
                && a.Id == id);

        if (!encontrado)
        {
            resultado.Error = "id".ErrorProcesoNoEncontrado();

        }
        else
        {
            resultado.Valido = true;
        }

        return resultado;
    }

    public override CuadroClasificacion ADTOFull(CuadroClasificacionActualizar actualizacion, CuadroClasificacion actual)
    {
        actual.Nombre = actualizacion.Nombre;
        return actual;
    }

    public override CuadroClasificacion ADTOFull(CuadroClasificacionInsertar insertar)
    {
        CuadroClasificacion cuadroClasificacion = new()
        {
            Id = Guid.NewGuid().ToString(),
            UOrgId = _contextoUsuario!.UOrgId,
            DominioId = _contextoUsuario!.DominioId!,
            Nombre = insertar.Nombre

        ,
        };
        return cuadroClasificacion;
    }

    public override CuadroClasificacionDespliegue ADTODespliegue(CuadroClasificacion data)
    {
        CuadroClasificacionDespliegue cuadroclasificacion = new()
        {
            Id = data.Id
        };
        return cuadroclasificacion;
    }



    #endregion

}

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
using pika.servicios.contenido.Extensiones;
using System.Collections.Specialized;
using System.Text.Json;

namespace pika.servicios.contenido.volumen;


[ServicioEntidadAPI(entidad: typeof(Volumen))]
public class ServicioVolumen : ServicioEntidadGenericaBase<Volumen, VolumenInsertar, VolumenActualizar, VolumenDespliegue, string>,
    IServicioEntidadAPI, IServicioVolumen
{

    private DbContextContenido localContext;

    public ServicioVolumen(DbContextContenido context, ILogger<ServicioVolumen> logger, IReflectorEntidadesAPI Reflector, IDistributedCache cache) : base(context, context.Volumenes, logger, Reflector, cache)
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
        var update = data.Deserialize<VolumenActualizar>(JsonAPIDefaults());
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
        var add = data.Deserialize<VolumenInsertar>(JsonAPIDefaults());
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

    #region Overrides para la personalización de la entidad Volumen

    public override async Task<ResultadoValidacion> ValidarInsertar(VolumenInsertar data)
    {
        ResultadoValidacion resultado = new();
        bool encontrado = await DB.Volumenes.AnyAsync(a => a.UOrgId == _contextoUsuario!.UOrgId
                && a.DominioId == _contextoUsuario.DominioId 
                && a.Nombre.ToLower() == data.Nombre.ToLower());

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


    public override async Task<ResultadoValidacion> ValidarEliminacion(string id, Volumen original, bool forzarEliminacion = false)
    {
        ResultadoValidacion resultado = new();
        bool encontrado = await DB.Volumenes.AnyAsync(a => a.UOrgId == _contextoUsuario!.UOrgId
                && a.DominioId == _contextoUsuario.DominioId
                && a.Id == id);

        if (!encontrado)
        {

            resultado.Error = "id".ErrorProcesoNoEncontrado();

        }
        else
        {
            bool EncontradoRepositorio = await DB.Repositorios.AnyAsync(a => a.VolumenId == id);
            bool EncontradoContenido = await DB.Contenidos.AnyAsync(a => a.VolumenId == id);
            if (EncontradoRepositorio || EncontradoContenido)
            {
                resultado.Error = "Id".ErrorConflict("El volumen se encuentra en uso");
            }
            else
            {
                resultado.Valido = true;
            }
        }

        return resultado;
    }


    public override async Task<ResultadoValidacion> ValidarActualizar(string id, VolumenActualizar actualizacion, Volumen original)
    {
        ResultadoValidacion resultado = new();
        bool encontrado = await DB.Volumenes.AnyAsync(a => a.UOrgId == _contextoUsuario!.UOrgId
                && a.DominioId == _contextoUsuario.DominioId
                && a.Id == id);

        if (!encontrado)
        {
            resultado.Error = "id".ErrorProcesoNoEncontrado();

        }
        else
        {
            // Verifica que no haya un registro con el mismo nombre para el mismo dominio y UO en un resgitrso diferente
            bool duplicado = await DB.Volumenes.AnyAsync(a => a.UOrgId == _contextoUsuario!.UOrgId
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


    public override Volumen ADTOFull(VolumenActualizar actualizacion, Volumen actual)
    {
        actual.Nombre = actualizacion.Nombre;
        actual.TipoGestorESId = actualizacion.TipoGestorESId;
        actual.TamanoMaximo = actualizacion.TamanoMaximo;
        actual.Activo = actualizacion.Activo;
        actual.EscrituraHabilitada = actualizacion.EscrituraHabilitada;
        return actual;
    }

    public override Volumen ADTOFull(VolumenInsertar data)
    {
        Volumen volumen = new()
        {
            Id = Guid.NewGuid().ToString(),
            UOrgId = _contextoUsuario!.UOrgId!,
            DominioId = _contextoUsuario!.DominioId!,
            Nombre = data.Nombre,
            TipoGestorESId = data.TipoGestorESId,
            TamanoMaximo = data.TamanoMaximo,
            Activo = data.Activo,
            EscrituraHabilitada = data.EscrituraHabilitada
        };
        return volumen;
    }

    public override VolumenDespliegue ADTODespliegue(Volumen data)
    {
        VolumenDespliegue volumenDespliegue = new()
        {
            Id = data.Id,
            Nombre = data.Nombre
        };
        return volumenDespliegue;
    }

    #endregion
}

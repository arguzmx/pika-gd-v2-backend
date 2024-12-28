using apigenerica.model.interpretes;
using apigenerica.model.modelos;
using apigenerica.model.reflectores;
using apigenerica.model.servicios;
using comunes.primitivas;
using extensibilidad.metadatos;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using pika.modelo.organizacion;
using pika.servicios.organizacion.dbcontext;
using System.Collections.Specialized;
using System.Text.Json;

#pragma warning disable CS8603 // Possible null reference return.
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
namespace pika.servicios.organizacion.unidadorganizacional;

[ServicioEntidadAPI(entidad: typeof(UnidadOrganizacional))]
public class ServicioUnidadOrganizacional : ServicioEntidadGenericaBase<UnidadOrganizacional, UnidadOrganizacionalInsertar, UnidadOrganizacionalActualizar, UnidadOrganizacionalDespliegue, string>,
IServicioEntidadAPI, IServicioUnidadOrganizacional
{
    private DbContextOrganizacion localContext;
    public ServicioUnidadOrganizacional(DbContextOrganizacion context, 
        ILogger<ServicioUnidadOrganizacional> logger, IReflectorEntidadesAPI Reflector,
        IDistributedCache cache) : base(context, context.UnidadesOrganizacionales,
            logger, Reflector, cache)
    {
        interpreteConsulta = new InterpreteConsultaMySQL();
        localContext = context;
    }

    /// <summary>
    /// Acceso al repositorio de UnidadOrganizacional local
    /// </summary>
    private DbContextOrganizacion DB { get { return (DbContextOrganizacion)_db; } }

    public bool RequiereAutenticacion => true;

    public async Task<Respuesta> ActualizarAPI(object id, JsonElement data, StringDictionary? parametros = null)
    {
        var update = data.Deserialize<UnidadOrganizacionalActualizar>(JsonAPIDefaults());
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
        var add = data.Deserialize<UnidadOrganizacionalInsertar>(JsonAPIDefaults());
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
    #region Overrides para la personalizacion de la entidad dominio

    public override async Task<ResultadoValidacion> ValidarInsertar(UnidadOrganizacionalInsertar data)
    {
        ResultadoValidacion resultado = new();

        bool encontrado = await DB.UnidadesOrganizacionales.AnyAsync(a => a.Nombre == data.Nombre && a.DominioId == data.DominioId);
        if (encontrado )
        {
            resultado.Error = "Nombre".ErrorProcesoDuplicado();
        }
        else
        {
            resultado.Valido = true;
        }
        return resultado;
    }


    public override async Task<ResultadoValidacion> ValidarEliminacion(string id, UnidadOrganizacional original, bool forzarEliminacion = false)
    {
        ResultadoValidacion resultado = new ();
        bool encontrado = await DB.UnidadesOrganizacionales.AnyAsync(a => a.Id == id);

        if (!encontrado)
        {
            resultado.Error = "Id".ErrorProcesoNoEncontrado();
        }
      
        else
        {
            bool EncontradoUsuarioUnidadOrganizacional = await DB.UsuariosUnidadesOrganizacionales.AnyAsync(a => a.UnidadOrganizacionalId==id);
            if(EncontradoUsuarioUnidadOrganizacional)
            {
                resultado.Error = "Id en uso verifique que este no se encuentre en UsuarioUnidadOrganizacional".Error409();
            }
            else
            {
                resultado.Valido = true;
            }
        }

        return resultado;
    }


    public override async Task<ResultadoValidacion> ValidarActualizar(string id, UnidadOrganizacionalActualizar actualizacion, UnidadOrganizacional original)
    {
        ResultadoValidacion resultado = new();
        bool encontrado = await DB.UnidadesOrganizacionales.AnyAsync(a => a.Id == id);
        if (!encontrado)
        {
            resultado.Error = "Id".ErrorProcesoNoEncontrado();
        }
        else
        {
            bool duplicado = await DB.UnidadesOrganizacionales.AnyAsync(a => a.Nombre == actualizacion.Nombre && a.DominioId == actualizacion.DominioId);
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


    public override UnidadOrganizacional ADTOFull(UnidadOrganizacionalActualizar actualizacion, UnidadOrganizacional actual)
    {
        actual.Nombre = actualizacion.Nombre;
        actual.DominioId = actualizacion.DominioId;
        return actual;
    }

    public override UnidadOrganizacional ADTOFull(UnidadOrganizacionalInsertar data)
    {
        UnidadOrganizacional archivo = new UnidadOrganizacional()
        {
            Id = Guid.NewGuid().ToString(),
            Nombre = data.Nombre,
            DominioId = data.DominioId,
        };
        return archivo;
    }

    public override UnidadOrganizacionalDespliegue ADTODespliegue(UnidadOrganizacional data)
    {
        UnidadOrganizacionalDespliegue archivo = new UnidadOrganizacionalDespliegue()
        {
            Id = data.Id,
            Nombre = data.Nombre,
            DominioId =data.DominioId
        };
        return archivo;
    }
    #endregion
}
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.
#pragma warning restore CS8603 // Possible null reference return.
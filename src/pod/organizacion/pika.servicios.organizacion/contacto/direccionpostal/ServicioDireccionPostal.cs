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
using pika.modelo.organizacion.Contacto;
using pika.servicios.organizacion.dbcontext;
using System.Collections.Specialized;
using System.Text.Json;

namespace pika.servicios.organizacion.contacto.direccionpostal;

[ServicioEntidadAPI(entidad: typeof(DireccionPostal))]
public class ServicioDireccionPostal : ServicioEntidadGenericaBase<DireccionPostal, DireccionPostalInsertar, DireccionPostalActualizar, DireccionPostalDespliegue, string>,
    IServicioEntidadAPI, IServicioDireccionPostal
{
    private DbContextOrganizacion localContext;

    public ServicioDireccionPostal(DbContextOrganizacion context, ILogger<ServicioDireccionPostal> logger, IReflectorEntidadesAPI Reflector, IDistributedCache cache) : base(context, context.DireccionesPostales, logger, Reflector, cache)
    {
        interpreteConsulta = new InterpreteConsultaMySQL();
        localContext = context;
    }


    /// <summary>
    /// Acceso al repositorio de gestipon documental local
    /// </summary>
    private DbContextOrganizacion DB { get { return (DbContextOrganizacion)_db; } }

    public bool RequiereAutenticacion => true;



    public async Task<Respuesta> ActualizarAPI(object id, JsonElement data, StringDictionary? parametros = null)
    {
        var update = data.Deserialize<DireccionPostalActualizar>(JsonAPIDefaults());
        return await this.Actualizar((string)id, update);
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
        var add = data.Deserialize<DireccionPostalInsertar>(JsonAPIDefaults());
        var temp = await this.Insertar(add);
        return temp.ReserializePayloadCamelCase(); ;
    }

    public async Task<RespuestaPayload<PaginaGenerica<object>>> PaginaAPI(Consulta consulta, StringDictionary? parametros = null)
    {
        var temp = await this.Pagina(consulta);
        return temp.ReserializePaginaCamelCase();
    }

    public async Task<RespuestaPayload<PaginaGenerica<object>>> PaginaDespliegueAPI(Consulta consulta, StringDictionary? parametros = null)
    {
        var temp = await this.PaginaDespliegue(consulta);
        return temp.ReserializePaginaCamelCase();
    }

    public async Task<RespuestaPayload<object>> UnicaPorIdAPI(object id, StringDictionary? parametros = null)
    {
        var temp = await this.UnicaPorId((string)id);
        return temp.ReserializePayloadCamelCase();
    }

    public async Task<RespuestaPayload<object>> UnicaPorIdDespliegueAPI(object id, StringDictionary? parametros = null)
    {
        var temp = await this.UnicaPorIdDespliegue((string)id);
        return temp.ReserializePayloadCamelCase();
    }
    #region Overrides para la personalización de la entidad Puesto

    public override async Task<ResultadoValidacion> ValidarInsertar(DireccionPostalInsertar data)
    {
        ResultadoValidacion resultado = new();
        bool encontrado = await DB.DireccionesPostales.AnyAsync(a => a.CP == data.CP && a.NoExterior == data.NoExterior && a.NoInterior == data.NoInterior  && a.UOrgId == _contextoUsuario!.UOrgId && a.DominioId == _contextoUsuario.DominioId);

        if (encontrado)
        {
            resultado.Error = "CodigoPostal , NoInterio, NoExterior ".ErrorProcesoDuplicado();
        }
        else
        {
            resultado.Valido = true;
        }

        return resultado;
    }


    public override async Task<ResultadoValidacion> ValidarEliminacion(string id, DireccionPostal original, bool forzarEliminacion = false)
    {
        ResultadoValidacion resultado = new();
        bool encontrado = await DB.DireccionesPostales.AnyAsync(a => a.Id == id && a.UOrgId == _contextoUsuario!.UOrgId && a.DominioId == _contextoUsuario.DominioId);

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


    public override async Task<ResultadoValidacion> ValidarActualizar(string id, DireccionPostalActualizar actualizacion, DireccionPostal original)
    {
        ResultadoValidacion resultado = new();

        bool duplicado = await DB.DireccionesPostales.AnyAsync(a => a.Id == id && a.UOrgId == _contextoUsuario!.UOrgId && a.DominioId == _contextoUsuario.DominioId);

        if (!duplicado)
        {
            resultado.Error = "Id".ErrorProcesoNoEncontrado();

        }
        else
        {
            resultado.Valido = true;
        }


        return resultado;
    }


    public override DireccionPostal ADTOFull(DireccionPostalActualizar actualizacion, DireccionPostal actual)
    {
        actual.Calle = actualizacion.Calle;
        actual.NoInterior = actualizacion.NoInterior;
        actual.NoExterior = actualizacion.NoExterior;
        actual.CP = actualizacion.CP;
        actual.Pais = actualizacion.Pais;
        actual.Estado = actualizacion.Estado;
        actual.Ciudad = actualizacion.Ciudad;
        actual.Referencia = actualizacion.Referencia;
        return actual;
    }

    public override DireccionPostal ADTOFull(DireccionPostalInsertar data)
    {
        DireccionPostal direccionPostal = new()
        {
            Id = Guid.NewGuid().ToString(),
            Calle = data.Calle,
            NoInterior = data.NoInterior,
            NoExterior = data.NoExterior,
            CP = data.CP,
            Pais = data.Pais,
            Estado = data.Estado,
            Ciudad = data.Ciudad,
            Referencia = data.Referencia,
            DominioId = _contextoUsuario.DominioId,
            UOrgId = _contextoUsuario.UOrgId


        };
        return direccionPostal;
    }

    public override DireccionPostalDespliegue ADTODespliegue(DireccionPostal data)
    {
        DireccionPostalDespliegue direccionPostalDespliegue = new()
        {
            Id = data.Id,
            Calle = data.Calle,
            NoInterior = data.NoInterior,
            NoExterior = data.NoExterior,
            CP = data.CP,
            Pais = data.Pais,
            Estado = data.Estado,
            Ciudad = data.Ciudad,
            Referencia = data.Referencia

        };
        return direccionPostalDespliegue;
    }

    #endregion
}

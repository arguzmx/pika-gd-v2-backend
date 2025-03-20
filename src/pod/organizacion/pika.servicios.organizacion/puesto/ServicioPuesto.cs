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

namespace pika.servicios.organizacion.puesto;

[ServicioEntidadAPI(entidad: typeof(Puesto))]
public class ServicioPuesto : ServicioEntidadGenericaBase<Puesto, PuestoInsertar, PuestoActualizar, PuestoDespliegue, string>,
    IServicioEntidadAPI, IServicioPuesto
{


    private DbContextOrganizacion localContext;

    public ServicioPuesto(DbContextOrganizacion context, ILogger<ServicioPuesto> logger, IReflectorEntidadesAPI Reflector, IDistributedCache cache) : base(context, context.Puestos, logger, Reflector, cache)
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
        var update = data.Deserialize<PuestoActualizar>(JsonAPIDefaults());
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
        var add = data.Deserialize<PuestoInsertar>(JsonAPIDefaults());
        var temp = await this.Insertar(add);
        return temp.ReserializePayloadCamelCase();
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

    public override async Task<ResultadoValidacion> ValidarInsertar(PuestoInsertar data)
    {
        ResultadoValidacion resultado = new();
        bool encontrado = await DB.Puestos.AnyAsync(a => a.Nombre == data.Nombre && a.UOrgId == _contextoUsuario!.UOrgId && a.DominioId == _contextoUsuario.DominioId);

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


    public override async Task<ResultadoValidacion> ValidarEliminacion(string id, Puesto original, bool forzarEliminacion = false)
    {
        ResultadoValidacion resultado = new();
        bool encontrado = await DB.Puestos.AnyAsync(a => a.Id == id && a.UOrgId == _contextoUsuario!.UOrgId && a.DominioId == _contextoUsuario.DominioId);

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


    public override async Task<ResultadoValidacion> ValidarActualizar(string id, PuestoActualizar actualizacion, Puesto original)
    {
        ResultadoValidacion resultado = new();

        bool duplicado = await DB.Puestos.AnyAsync(a => a.Id != id && a.Nombre.Equals(actualizacion.Nombre) && a.UOrgId == _contextoUsuario!.UOrgId && a.DominioId == _contextoUsuario.DominioId);

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


    public override Puesto ADTOFull(PuestoActualizar actualizacion, Puesto actual)
    {
        actual.Nombre = actualizacion.Nombre;
        actual.Clave = actualizacion.Clave;
        return actual;
    }

    public override Puesto ADTOFull(PuestoInsertar data)
    {
        Puesto contenido = new()
        {
            Id = Guid.NewGuid().ToString(),
            Nombre = data.Nombre,
            Clave = data.Clave,
            DominioId = _contextoUsuario.DominioId,
            UOrgId = _contextoUsuario.UOrgId
            

        };
        return contenido;
    }

    public override PuestoDespliegue ADTODespliegue(Puesto data)
    {
        PuestoDespliegue puestoDespliegue = new()
        {
            Id = data.Id,
            Nombre = data.Nombre,
            Clave = data.Clave,
    
        };
        return puestoDespliegue;
    }

    #endregion
}

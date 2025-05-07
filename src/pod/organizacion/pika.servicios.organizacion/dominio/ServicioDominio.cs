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
namespace pika.servicios.organizacion.dominio;

[ServicioEntidadAPI(entidad: typeof(Dominio))]
//[RequisitosEjecuion(requiereAutenticacion: true)]
public class ServicioDominio : ServicioEntidadGenericaBase<Dominio, DominioInsertar, DominioActualizar, DominioDespliegue, string>,
    IServicioEntidadAPI, IServicioDominio
{
    private DbContextOrganizacion localContext;
    public ServicioDominio(DbContextOrganizacion context, ILogger<ServicioDominio> logger,
        IReflectorEntidadesAPI Reflector, IDistributedCache cache) 
        : base(context, context.Dominios, logger, Reflector, cache)
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
        var update = data.Deserialize<DominioActualizar>(JsonAPIDefaults());
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
            var add = data.Deserialize<DominioInsertar>(JsonAPIDefaults());
            var temp = await this.Insertar(add, parametros);
            return temp.ReserializePayloadCamelCase();
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
        return temp.ReserializePayloadCamelCase();
    }

    public async Task<RespuestaPayload<object>> UnicaPorIdDespliegueAPI(object id, StringDictionary? parametros = null)
    {
        var temp = await this.UnicaPorIdDespliegue((string)id, parametros);
        return temp.ReserializePayloadCamelCase();
    }

    #region Overrides para la personalizacion de la entidad dominio

    public override async Task<ResultadoValidacion> ValidarInsertar(DominioInsertar data)
    {
        ResultadoValidacion resultado = new ();

        // VErifica que el usuario no tengo otro dominio con el mismo nombre en un registro diferente al de actualizacion
        bool encontrado = await DB.Dominios.AnyAsync(a => a.Nombre == data.Nombre
        && a.UsuarioId == this._contextoUsuario!.UsuarioId);

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


    public override async Task<ResultadoValidacion> ValidarEliminacion(string id, Dominio original, bool forzarEliminacion = false)
    {
        ResultadoValidacion resultado = new();
        bool encontrado = await DB.Dominios.AnyAsync(a => a.Id==id);
        
        if (!encontrado)
        {
            
            resultado.Error = "Id".ErrorProcesoNoEncontrado();

        }
        else
        {
            bool EncontradoUsuarioDominio = await DB.UsuarioDominios.AnyAsync(a => a.DominioId == id);
            bool EncontradoUnidadOrganizacional = await DB.UnidadesOrganizacionales.AnyAsync(a => a.DominioId == id);
            bool EncontradoUsuarioUnidadOrganizacional = await DB.UsuariosUnidadesOrganizacionales.AnyAsync(a => a.DominioId == id);

            if (EncontradoUsuarioDominio || EncontradoUnidadOrganizacional || EncontradoUsuarioUnidadOrganizacional)
            {
                resultado.Error = "Id en uso verifique que este no se encuentre en UsuarioDominio,UnidadOrganizacional O UsuarioUnidadOrganizacional".Error409();
            }
            else
            { resultado.Valido = true; }
        }

        return resultado;
    }
 
    

    public override async Task<ResultadoValidacion> ValidarActualizar(string id, DominioActualizar actualizacion, Dominio original)
    {
        ResultadoValidacion resultado = new();
        bool encontrado = await DB.Dominios.AnyAsync(a => a.Id == id);

        resultado.Valido = false;
        if (!encontrado)
        {
            resultado.Error = "id".ErrorProcesoNoEncontrado();

        }
        else
        {

            // VErifica que el usuario no tengo otro dominio con el mismo nombre en un registro diferente al de actualizacion
            bool duplicado = await DB.Dominios.AnyAsync(a => a.Nombre== actualizacion.Nombre 
            && a.UsuarioId == this._contextoUsuario!.UsuarioId && a.Id != id);

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


    public override Dominio ADTOFull(DominioActualizar actualizacion, Dominio actual)
    {
        actual.Nombre = actualizacion.Nombre;
        actual.Activo = actualizacion.Activo;
        actual.UsuarioId = _contextoUsuario!.UsuarioId!;
        return actual;
    }

    public override Dominio ADTOFull(DominioInsertar data)
    {
        Dominio archivo = new Dominio()
        {
            Id =  Guid.NewGuid().ToString(),
            Nombre = data.Nombre,
            Activo = data.Activo,
            UsuarioId = _contextoUsuario.UsuarioId
        };
        return archivo;
    }

    public override DominioDespliegue ADTODespliegue(Dominio data)
    {
        DominioDespliegue archivo = new DominioDespliegue()
        {
            Id = data.Id,
            Activo = data.Activo,
            FechaCreacion = data.FechaCreacion,
            Nombre = data.Nombre,
            UsuarioId = data.UsuarioId
        };
        return archivo;
    }

    #endregion

}
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.
#pragma warning restore CS8603 // Possible null reference return.
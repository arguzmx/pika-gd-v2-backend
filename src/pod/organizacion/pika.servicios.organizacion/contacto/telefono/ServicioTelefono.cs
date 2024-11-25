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

namespace pika.servicios.organizacion.contacto.telefono;



[ServicioEntidadAPI(entidad: typeof(Telefono))]
public class ServicioTelefono : ServicioEntidadGenericaBase<Telefono, TelefonoInsertar, TelefonoActualizar, TelefonoDespliegue, string>,
    IServicioEntidadAPI, IServicioTelefono
{

    private DbContextOrganizacion localContext;

    public ServicioTelefono(DbContextOrganizacion context, ILogger<ServicioTelefono> logger, IReflectorEntidadesAPI Reflector, IDistributedCache cache) : base(context, context.Telefonos, logger, Reflector, cache)
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
        var update = data.Deserialize<TelefonoActualizar>(JsonAPIDefaults());
        return await this.Actualizar((string)id, update);
    }

    public async Task<Respuesta> EliminarAPI(object id, StringDictionary? parametros = null)
    {
        return await this.Eliminar((string)id);
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
        var add = data.Deserialize<TelefonoInsertar>(JsonAPIDefaults());
        var temp = await this.Insertar(add);
        RespuestaPayload<object> respuesta = JsonSerializer.Deserialize<RespuestaPayload<object>>(JsonSerializer.Serialize(temp));
        return respuesta;
    }

    public async Task<RespuestaPayload<PaginaGenerica<object>>> PaginaAPI(Consulta consulta, StringDictionary? parametros = null)
    {
        var temp = await this.Pagina(consulta);
        RespuestaPayload<PaginaGenerica<object>> respuesta = JsonSerializer.Deserialize<RespuestaPayload<PaginaGenerica<object>>>(JsonSerializer.Serialize(temp));

        return respuesta;
    }

    public async Task<RespuestaPayload<PaginaGenerica<object>>> PaginaDespliegueAPI(Consulta consulta, StringDictionary? parametros = null)
    {
        var temp = await this.PaginaDespliegue(consulta);
        RespuestaPayload<PaginaGenerica<object>> respuesta = JsonSerializer.Deserialize<RespuestaPayload<PaginaGenerica<object>>>(JsonSerializer.Serialize(temp));
        return respuesta;
    }

    public async Task<RespuestaPayload<object>> UnicaPorIdAPI(object id, StringDictionary? parametros = null)
    {
        var temp = await this.UnicaPorId((string)id);
        RespuestaPayload<object> respuesta = JsonSerializer.Deserialize<RespuestaPayload<object>>(JsonSerializer.Serialize(temp));
        return respuesta;
    }

    public async Task<RespuestaPayload<object>> UnicaPorIdDespliegueAPI(object id, StringDictionary? parametros = null)
    {
        var temp = await this.UnicaPorIdDespliegue((string)id);

        RespuestaPayload<object> respuesta = JsonSerializer.Deserialize<RespuestaPayload<object>>(JsonSerializer.Serialize(temp));
        return respuesta;
    }

    #region Overrides para la personalizacion de la entidad telefono

    public override async Task<ResultadoValidacion> ValidarInsertar(TelefonoInsertar data)
    {
        ResultadoValidacion resultado = new();

        // VErifica que el usuario no tengo otro dominio con el mismo nombre en un registro diferente al de actualizacion
        bool encontrado = await DB.Telefonos.AnyAsync(a => _contextoUsuario.DominioId != _contextoUsuario.DominioId && a.UOrgId == _contextoUsuario!.UOrgId && a.DominioId == _contextoUsuario.DominioId);

        if (encontrado)
        {
            resultado.Error = "DominioId".ErrorProcesoNoEncontrado();

        }
        else
        {
            resultado.Valido = true;
        }

        return resultado;
    }


    public override async Task<ResultadoValidacion> ValidarEliminacion(string id, Telefono original)
    {
        ResultadoValidacion resultado = new();
        bool encontrado = await DB.Telefonos.AnyAsync(a => a.Id == id && a.UOrgId == _contextoUsuario!.UOrgId && a.DominioId == _contextoUsuario.DominioId);

        if (!encontrado)
        {

            resultado.Error = "Id".ErrorProcesoNoEncontrado();

        }
        else
        {
            resultado.Valido = true;
        }

        return resultado;
    }



    public override async Task<ResultadoValidacion> ValidarActualizar(string id, TelefonoActualizar actualizacion, Telefono original)
    {
        ResultadoValidacion resultado = new();
        bool encontrado = await DB.Telefonos.AnyAsync(a => a.Id == id && a.UOrgId == _contextoUsuario!.UOrgId && a.DominioId == _contextoUsuario.DominioId);

        resultado.Valido = false;
        if (!encontrado)
        {
            resultado.Error = "id".ErrorProcesoNoEncontrado();

        }
        else
        {

            // VErifica que el usuario no tengo otro dominio con el mismo nombre en un registro diferente al de actualizacion
            
                resultado.Valido = true;
            
        }

        return resultado;
    }


    public override Telefono ADTOFull(TelefonoActualizar actualizacion, Telefono actual)
    {
       
        actual.Numero = actualizacion.Numero;
        actual.Extension = actualizacion.Extension;
        actual.Horario = actualizacion.Horario;
        return actual;
    }

    public override Telefono ADTOFull(TelefonoInsertar data)
    {
        Telefono telefono = new Telefono()
        {
            Id = Guid.NewGuid().ToString(),
            Numero = data.Numero,
            Extension = data.Extension,
            Horario = data.Horario,
            DominioId = _contextoUsuario.DominioId,
            UOrgId = _contextoUsuario.UOrgId
        };
        return telefono;
    }

    public override TelefonoDespliegue ADTODespliegue(Telefono data)
    {
        TelefonoDespliegue telefonoDespliegue = new TelefonoDespliegue()
        {
            Id = data.Id,
            Numero = data.Numero,
            Extension = data.Extension,
            Horario = data.Horario
        };
        return telefonoDespliegue;
    }

    #endregion

}

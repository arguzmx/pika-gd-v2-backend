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


namespace pika.servicios.organizacion.usuariounidadorganizacional;


[ServicioEntidadAPI(entidad: typeof(UsuarioUnidadOrganizacional))]
public class ServicioUsuarioUnidadOrganizacional : ServicioEntidadGenericaBase<UsuarioUnidadOrganizacional, UsuarioUnidadOrganizacionalInsertar, UsuarioUnidadOrganizacionalActualizar, UsuarioUnidadOrganizacionalDespliegue, string>,
    IServicioEntidadAPI, IServicioUsuarioUnidadOrganizacional
{
    public ServicioUsuarioUnidadOrganizacional(DbContextOrganizacion context, 
        ILogger<ServicioUsuarioUnidadOrganizacional> logger, IReflectorEntidadesAPI Reflector, IDistributedCache cache) : base(
            context, context.UsuariosUnidadesOrganizacionales, logger, Reflector, cache )
    {
    }


    /// <summary>
    /// Acceso al repositorio de gestipon documental local
    /// </summary>
    private DbContextOrganizacion DB { get { return (DbContextOrganizacion)_db; } }


    public bool RequiereAutenticacion => true;

    public async Task<Respuesta> ActualizarAPI(object id, JsonElement data, StringDictionary? parametros = null)
    {
        //un usuario de dominio no se puede actualizar,
        //solo se crea o se elimina la relación de hecho el
        //método de actualización en el crud debe devolver NotImplementedException
        throw new NotImplementedException();
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
        var add = data.Deserialize<UsuarioUnidadOrganizacionalInsertar>(JsonAPIDefaults());
        var temp = await this.Insertar(add);
        RespuestaPayload<object> respuesta = JsonSerializer.Deserialize<RespuestaPayload<object>>(JsonSerializer.Serialize(temp));
        return respuesta;
    }

    public async Task<RespuestaPayload<PaginaGenerica<object>>> PaginaAPI(Consulta consulta, StringDictionary? parametros = null)
    {
        //un usuario de dominio no se puede actualizar,
        //solo se crea o se elimina la relación de hecho el
        //método de actualización en el crud debe devolver NotImplementedException
        throw new NotImplementedException();
    }

    public async Task<RespuestaPayload<PaginaGenerica<object>>> PaginaDespliegueAPI(Consulta consulta, StringDictionary? parametros = null)
    {
        //un usuario de dominio no se puede actualizar,
        //solo se crea o se elimina la relación de hecho el
        //método de actualización en el crud debe devolver NotImplementedException
        throw new NotImplementedException();
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
    #region Overrides para la personalizacion de la entidad dominio
    //Validar si UnidadOrganizacionalId EXIXTE 
    public override async Task<ResultadoValidacion> ValidarInsertar(UsuarioUnidadOrganizacionalInsertar data)
    {
        ResultadoValidacion resultado = new();

        resultado.Valido = false;
        bool encontrado = await DB.UnidadesOrganizacionales.AnyAsync(a => a.Id == data.UnidadOrganizacionalId);
        bool encontrado2 = await DB.Dominios.AnyAsync(a => a.Id == data.DominioId);
       

        if (!encontrado || !encontrado2)
        {
            resultado.Error = "UnidadOrganizacionalId".ErrorProcesoNoEncontrado();
            if(!encontrado2)
            {
                resultado.Error = "DominioId".ErrorProcesoNoEncontrado();
            }

        }
        else
        {
            resultado.Valido = true;
        }

        return resultado;
    }

    // Validar si se encuentra el id
    public override async Task<ResultadoValidacion> ValidarEliminacion(string id, UsuarioUnidadOrganizacional original, bool forzarEliminacion = false)
    {
        ResultadoValidacion resultado = new();
        bool encontrado = await DB.UsuariosUnidadesOrganizacionales.AnyAsync(a => a.Id == id);

        resultado.Valido = false;
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

    // validacion inhabilitada
    public override async Task<ResultadoValidacion> ValidarActualizar(string id, UsuarioUnidadOrganizacionalActualizar actualizacion, UsuarioUnidadOrganizacional original)
    {
        ResultadoValidacion resultado = new();
        resultado.Valido = false;
        resultado.Error = "Metodo Inhabilitado".Error409();
        return resultado;
    }


    public override UsuarioUnidadOrganizacional ADTOFull(UsuarioUnidadOrganizacionalActualizar actualizacion, UsuarioUnidadOrganizacional actual)
    {

        return actual;
    }

    public override UsuarioUnidadOrganizacional ADTOFull(UsuarioUnidadOrganizacionalInsertar data)
    {
        UsuarioUnidadOrganizacional archivo = new UsuarioUnidadOrganizacional()
        {
            Id = Guid.NewGuid().ToString(),
            UsuarioId = data.UsuarioId,
            DominioId = data.DominioId,
            UnidadOrganizacionalId=data.UnidadOrganizacionalId
        };
        return archivo;
    }

    public override UsuarioUnidadOrganizacionalDespliegue ADTODespliegue(UsuarioUnidadOrganizacional data)
    {
        UsuarioUnidadOrganizacionalDespliegue archivo = new UsuarioUnidadOrganizacionalDespliegue()
        {
            Id = data.Id,
            UsuarioId =data.UsuarioId,
            DominioId= data.DominioId,
            UnidadOrganizacionalId= data.UnidadOrganizacionalId
        };
        return archivo;
    }

    #endregion
}

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

namespace pika.servicios.organizacion.usuariodominio;


[ServicioEntidadAPI(entidad: typeof(UsuarioDominio))]
public class ServicioUsuarioDominio : ServicioEntidadGenericaBase<UsuarioDominio, UsuarioDominioInsertar, UsuarioDominioActualizar, UsuarioDominioDespliegue, string>,
    IServicioEntidadAPI, IServicioUsuarioDominio
{
    public ServicioUsuarioDominio(DbContextOrganizacion context, ILogger<ServicioUsuarioDominio> logger,
        IReflectorEntidadesAPI Reflector, IDistributedCache cache) : base(context, context.UsuarioDominios, logger, Reflector, cache )
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
        var add = data.Deserialize<UsuarioDominioInsertar>(JsonAPIDefaults());
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

    public async Task<RespuestaPayload<PaginaGenerica<object>>> PaginaHijoAPI(Consulta consulta, string tipoPadre, string id)
    {
        var temp = await this.PaginaHijo(consulta, tipoPadre, id);
        RespuestaPayload<PaginaGenerica<object>> respuesta = JsonSerializer.Deserialize<RespuestaPayload<PaginaGenerica<object>>>(JsonSerializer.Serialize(temp));
        return respuesta;
    }

    public async Task<RespuestaPayload<PaginaGenerica<object>>> PaginaHijosDespliegueAPI(Consulta consulta, string tipoPadre, string id)
    {
        var temp = await this.PaginaHijosDespliegue(consulta, tipoPadre, id);
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




    #region Overrides para la personalizacion de la entidad dominio
    //Validar si DominioId Existe
    public override async Task<ResultadoValidacion> ValidarInsertar(UsuarioDominioInsertar data)
    {
        ResultadoValidacion resultado = new();

        bool encontrado = await DB.Dominios.AnyAsync(a => a.Id == data.DominioId);

        if (!encontrado)
        {
            resultado.Error = "DominioId".ErrorProcesoNoEncontrado();

        }
        else
        {
            resultado.Valido = true;
        }

        return resultado;
    }

    // Validar si se encuentra el id
    public override async Task<ResultadoValidacion> ValidarEliminacion(string id, UsuarioDominio original)
    {
        ResultadoValidacion resultado = new();
        bool encontrado = await DB.UsuarioDominios.AnyAsync(a => a.Id == id);

        resultado.Valido = false;
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

    // validacion inhabilitada
    public override async Task<ResultadoValidacion> ValidarActualizar(string id, UsuarioDominioActualizar actualizacion, UsuarioDominio original)
    {
        ResultadoValidacion resultado = new();
        resultado.Valido = false;
        resultado.Error = "Metodo Inhabilitado".Error409();
        return resultado;
    }


    public override UsuarioDominio ADTOFull(UsuarioDominioActualizar actualizacion, UsuarioDominio actual)
    {
       
        return actual;
    }

    public override UsuarioDominio ADTOFull(UsuarioDominioInsertar data)
    {
        UsuarioDominio archivo = new UsuarioDominio()
        {
            Id = Guid.NewGuid().ToString(),
            UsuarioId = data.UsuarioId,
            DominioId = data.DominioId
        };
        return archivo;
    }

    public override UsuarioDominioDespliegue ADTODespliegue(UsuarioDominio data)
    {
        UsuarioDominioDespliegue archivo = new UsuarioDominioDespliegue()
        {
            Id = data.Id,
            UsuarioId= data.UsuarioId,
            DominioId = data.DominioId

        };
        return archivo;
    }

    #endregion
}

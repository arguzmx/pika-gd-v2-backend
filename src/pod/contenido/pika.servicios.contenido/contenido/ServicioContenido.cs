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

namespace pika.servicios.contenido;


[ServicioEntidadAPI(entidad: typeof(Contenido))]
public class ServicioContenido : ServicioEntidadGenericaBase<Contenido, ContenidoInsertar, ContenidoActualizar, ContenidoDespliegue, string>,
    IServicioEntidadAPI, IServicioContenido
{
    private DbContextContenido localContext;

    public ServicioContenido(DbContextContenido context, ILogger<ServicioContenido> logger, IReflectorEntidadesAPI Reflector, IDistributedCache cache) : base(context, context.Contenidos, logger, Reflector, cache)
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
        var update = data.Deserialize<ContenidoActualizar>(JsonAPIDefaults());
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
        var add = data.Deserialize<ContenidoInsertar>(JsonAPIDefaults());
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

    #region Overrides para la personalización de la entidad Repositorio

    public override async Task<ResultadoValidacion> ValidarInsertar(ContenidoInsertar data)
    {
        ResultadoValidacion resultado = new();
        bool encontrado = await DB.Contenidos.AnyAsync(a => a.Nombre == data.Nombre);

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


    public override async Task<ResultadoValidacion> ValidarEliminacion(string id, Contenido original)
    {
        ResultadoValidacion resultado = new();
        bool encontrado = await DB.Contenidos.AnyAsync(a => a.Id == id);

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


    public override async Task<ResultadoValidacion> ValidarActualizar(string id, ContenidoActualizar actualizacion, Contenido original)
    {
        ResultadoValidacion resultado = new();

        bool duplicado = await DB.Contenidos.AnyAsync(a => a.Id != id && a.Nombre.Equals(actualizacion.Nombre));

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


    public override Contenido ADTOFull(ContenidoActualizar actualizacion, Contenido actual)
    {
        actual.Id = actualizacion.Id;
        actual.Nombre = actualizacion.Nombre;
        actual.IdExterno = actualizacion.IdExterno;
        return actual;
    }

    public override Contenido ADTOFull(ContenidoInsertar data)
    {
        Contenido contenido = new()
        {
            Id = Guid.NewGuid().ToString(),
            Nombre = data.Nombre,
            RepositorioId = data.RepositorioId,
            CreadorId = "seobtienedejwt",
            FechaCreacion = DateTime.Now,
            VolumenId = data.VolumenId,
            CarpetaId = data.CarpetaId,
            TipoElemento = data.TipoElemento,
            IdExterno = data.IdExterno,
            PermisoId = "permiso09585854",

        };
        return contenido;
    }

    public override ContenidoDespliegue ADTODespliegue(Contenido data)
    {
        ContenidoDespliegue contenidoDespliegue = new()
        {
            Id = data.Id,
            Nombre = data.Nombre,
            CreadorId = data.CreadorId,
            FechaCreacion = data.FechaCreacion,
            ConteoAnexos = data.ConteoAnexos,
            TamanoBytes = data.TamanoBytes,
            VolumenId = data.VolumenId,
            CarpetaId = data.CarpetaId,
            TipoElemento = data.TipoElemento,
            IdExterno = data.IdExterno
        };
        return contenidoDespliegue;
    }
    #endregion
}
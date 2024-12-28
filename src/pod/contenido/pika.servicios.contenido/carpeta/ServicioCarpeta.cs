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

#pragma warning disable CS8603 // Possible null reference return.
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
namespace pika.servicios.contenido;

[ServicioEntidadAPI(entidad: typeof(Carpeta))]
public class ServicioCarpeta : ServicioEntidadGenericaBase<Carpeta, CarpetaInsertar, CarpetaActualizar, CarpetaDespliegue, string>,
    IServicioEntidadAPI, IServicioCarpeta
{

    private DbContextContenido localContext;

    public ServicioCarpeta(DbContextContenido context, ILogger<ServicioCarpeta> logger, IReflectorEntidadesAPI Reflector, IDistributedCache cache) : base(context, context.Carpetas, logger, Reflector, cache)
    {
        interpreteConsulta = new InterpreteConsultaMySQL();
        localContext = context;
    }


    /// <summary>
    /// Acceso al repositorio de gestipon documental local
    /// </summary>
    private DbContextContenido DB { get { return (DbContextContenido)_db; } }

    public bool RequiereAutenticacion => true;

    private const string REPOID = "n0Id";
    private string Valor(StringDictionary? parametros, string clave)
    {
        string valor = null;
        if (parametros != null) {

            if (parametros.ContainsKey(clave)) { valor = parametros[clave]; } 
        }
        return valor;
    }

    private string RepositorioId;


    public async Task<Respuesta> ActualizarAPI(object id, JsonElement data, StringDictionary? parametros = null)
    {
        RepositorioId = Valor(parametros, REPOID);
        var update = data.Deserialize<CarpetaActualizar>(JsonAPIDefaults());
        return await this.Actualizar((string)id, update, parametros);
    }

    public async Task<Respuesta> EliminarAPI(object id, StringDictionary? parametros = null, bool forzarEliminacion = false)
    {
        RepositorioId = Valor(parametros, REPOID);
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
        RepositorioId = Valor(parametros, REPOID);
        var add = data.Deserialize<CarpetaInsertar>(JsonAPIDefaults());
        add!.RepositorioId = RepositorioId;
        var temp = await this.Insertar(add, parametros);
        RespuestaPayload<object> respuesta = JsonSerializer.Deserialize<RespuestaPayload<object>>(JsonSerializer.Serialize(temp));
        return respuesta;
    }

    public async Task<RespuestaPayload<PaginaGenerica<object>>> PaginaAPI(Consulta consulta, StringDictionary? parametros = null)
    {
        RepositorioId = Valor(parametros, REPOID);
        var temp = await this.Pagina(consulta, parametros);
        RespuestaPayload<PaginaGenerica<object>> respuesta = JsonSerializer.Deserialize<RespuestaPayload<PaginaGenerica<object>>>(JsonSerializer.Serialize(temp));
        return respuesta;
    }

    public async Task<RespuestaPayload<PaginaGenerica<object>>> PaginaDespliegueAPI(Consulta consulta, StringDictionary? parametros = null)
    {
        RepositorioId = Valor(parametros, REPOID);
        var temp = await this.PaginaDespliegue(consulta, parametros);
        RespuestaPayload<PaginaGenerica<object>> respuesta = JsonSerializer.Deserialize<RespuestaPayload<PaginaGenerica<object>>>(JsonSerializer.Serialize(temp));
        return respuesta;
    }

    public async Task<RespuestaPayload<object>> UnicaPorIdAPI(object id, StringDictionary? parametros = null)
    {
        RepositorioId = Valor(parametros, REPOID);
        var temp = await this.UnicaPorId((string)id, parametros);
        RespuestaPayload<object> respuesta = JsonSerializer.Deserialize<RespuestaPayload<object>>(JsonSerializer.Serialize(temp));
        return respuesta;
    }

    public async Task<RespuestaPayload<object>> UnicaPorIdDespliegueAPI(object id, StringDictionary? parametros = null)
    {
        RepositorioId = Valor(parametros, REPOID);
        var temp = await this.UnicaPorIdDespliegue((string)id, parametros);

        RespuestaPayload<object> respuesta = JsonSerializer.Deserialize<RespuestaPayload<object>>(JsonSerializer.Serialize(temp));
        return respuesta;
    }

    public override async Task<RespuestaPayload<List<NodoArbol<object>>>> Arbol(string? raizId = null, bool parcial = true, bool incluirPayload = false, StringDictionary? parametros = null)
    {
        RepositorioId = Valor(parametros, REPOID);
        RespuestaPayload<List<NodoArbol<object>>> respuesta = new ();
        var carpetas = ArbolRecursivo(raizId, parcial, incluirPayload, RepositorioId);
        List<NodoArbol<object>> nodos = [];
        foreach (var c in carpetas)
        {
            NodoArbol<object> nodo = new() { Id = c.Id, PadreId = c.CarpetaPadreId, Texto = c.Nombre };
            if(incluirPayload)
            {
                nodo.Payload = c;
            }
            nodos.Add(nodo);
        }
        respuesta.Payload = nodos;  
        respuesta.Ok = true;
        return respuesta;
    }

    private List<Carpeta> ArbolRecursivo(string? padreId , bool parcial, bool incluirPayload, string repositorioId)
    {
        List<Carpeta> carpetas = [];
        if(string.IsNullOrEmpty(padreId))
        {
            carpetas = DB.Carpetas.Where(c=>c.EsRaiz == true && c.RepositorioId == repositorioId).ToList();  

        } else
        {
            carpetas = DB.Carpetas.Where(c => c.CarpetaPadreId == padreId && c.RepositorioId == repositorioId).ToList();
        }

        if (carpetas.Count > 0) { 
        
            if(!parcial)
            {
                foreach (var c in carpetas) { 
                    var tmp = ArbolRecursivo(c.Id, parcial, incluirPayload, repositorioId);
                    if (tmp.Count > 0) {
                        carpetas.AddRange(tmp);
                    }
                }
            }
        }
        return carpetas;
    }


    #region Overrides para la personalización de la entidad Carpeta y validaciones

    public override Carpeta ADTOFull(CarpetaActualizar actualizacion, Carpeta actual)
    {
        actual.Nombre = actualizacion.Nombre;
        actual.CarpetaPadreId = actualizacion.CarpetaPadreId;
        return actual;
    }

    public override Carpeta ADTOFull(CarpetaInsertar data)
    {
        Carpeta carpeta = new()
        {
            Id = Guid.NewGuid().ToString(),
            RepositorioId = RepositorioId,
            CreadorId = _contextoUsuario!.UsuarioId!,
            FechaCreacion = DateTime.Now,
            Nombre = data.Nombre,
            CarpetaPadreId = data.CarpetaPadreId,
            EsRaiz = data.CarpetaPadreId == null,
            PermisoId = null
        };
        return carpeta;
    }

    public override CarpetaDespliegue ADTODespliegue(Carpeta data)
    {
        CarpetaDespliegue carpetaDespliegue = new()
        {
            Id = data.Id,
            CreadorId = data.CreadorId,
            FechaCreacion = data.FechaCreacion,
            Nombre = data.Nombre,
            CarpetaPadreId = data.CarpetaPadreId
        };
        return carpetaDespliegue;
    }


    public override async Task<ResultadoValidacion> ValidarInsertar(CarpetaInsertar data)
    {
        ErrorProceso error = null;

        if (data.RepositorioId == null)
        {
            error = "RepositorioId".ErrorBadRequest("No pude ser nulo");
        }
        else
        {
            bool repoValido = await RepositorioValido(RepositorioId);
            if (repoValido)
            {

                bool duplicado = await CarpetaDuplicada(data.Nombre, data.CarpetaPadreId, null, RepositorioId, false);
                if (duplicado)
                {
                    error = "Nombre".ErrorProcesoDuplicado();
                }
            }
            else
            {
                error = "RepositorioId".ErrorBadRequest("Repositorio inexistente");
            }
        }
        return new ResultadoValidacion() { Valido = error == null, Error = error };
    }


    public override async Task<ResultadoValidacion> ValidarActualizar(string id, CarpetaActualizar actualizacion, Carpeta original)
    {
        ResultadoValidacion resultado = new();
        ErrorProceso error = null;

        bool repoValido = await RepositorioValido(RepositorioId);
        if (repoValido)
        {
            bool duplicado = await CarpetaDuplicada(actualizacion.Nombre, actualizacion.CarpetaPadreId, original.Id, RepositorioId, true);
            if (duplicado)
            {
                error = "Nombre".ErrorProcesoDuplicado();
            }
        }
        else
        {
            error = "RepositorioId".ErrorBadRequest("Repositorio inexistente");
        }
        return new ResultadoValidacion() { Valido = error == null, Error = error };
    }

    public override async Task<ResultadoValidacion> ValidarEliminacion(string id, Carpeta original, bool forzarEliminacion = false)
    {
        ErrorProceso error = null;
        var carpeta = await this.DB.Carpetas.FirstOrDefaultAsync(c => c.RepositorioId == RepositorioId && c.Id == id);
        if (carpeta != null)
        {
            if (!forzarEliminacion)
            {
                var hijos = DB.Carpetas.Any(c => c.CarpetaPadreId == id) || DB.Contenidos.Any(c => c.CarpetaId == id);
                if (hijos)
                {
                    error = "Id".ErrorConflict("carpeta no vacía");
                }
            }
        }
        else
        {
            error = "Id".ErrorNotFound("carpeta inexistente");
        }
        return new ResultadoValidacion() { Valido = error == null, Error = error };
    }

    private async Task<bool> RepositorioValido(string repositorioId)
    {
        return await this.DB.Repositorios.AnyAsync(r => r.Id == repositorioId);
    }

    private async Task<bool> CarpetaDuplicada(string nombre, string? padreId, string? carpetaId, string repositorioId, bool update)
    {
        Carpeta carpeta;
        if (update)
        {
            if (string.IsNullOrEmpty(padreId))
            {
                carpeta = await this.DB.Carpetas.FirstOrDefaultAsync(c => c.RepositorioId == repositorioId
                && c.Id != carpetaId && c.Nombre.Equals(nombre, StringComparison.InvariantCultureIgnoreCase) && c.EsRaiz == true);
            }
            else
            {
                carpeta = await this.DB.Carpetas.FirstOrDefaultAsync(c => c.RepositorioId == repositorioId
                && c.Id != carpetaId && c.Nombre.Equals(nombre, StringComparison.InvariantCultureIgnoreCase) && c.CarpetaPadreId == padreId);
            }
        }
        else
        {
            if (string.IsNullOrEmpty(padreId))
            {
                carpeta = await this.DB.Carpetas.FirstOrDefaultAsync(c => c.RepositorioId == repositorioId && c.Nombre.Equals(nombre, StringComparison.InvariantCultureIgnoreCase) && c.EsRaiz == true);
            }
            else
            {
                carpeta = await this.DB.Carpetas.FirstOrDefaultAsync(c => c.RepositorioId == repositorioId && c.Nombre.Equals(nombre, StringComparison.InvariantCultureIgnoreCase) && c.CarpetaPadreId == padreId);
            }
        }

        return carpeta != null;
    }

    #endregion

}
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.
#pragma warning restore CS8603 // Possible null reference return.

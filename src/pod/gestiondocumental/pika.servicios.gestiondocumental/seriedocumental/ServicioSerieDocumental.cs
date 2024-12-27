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
using pika.modelo.gestiondocumental;
using System.Collections.Specialized;
using System.Text.Json;

namespace pika.servicios.gestiondocumental.seriedocumental;

[ServicioEntidadAPI(entidad: typeof(SerieDocumental))]
public class ServicioSerieDocumental : ServicioEntidadGenericaBase<SerieDocumental, SerieDocumentalInsertar, SerieDocumentalActualizar, SerieDocumentalDespliegue, string>,
    IServicioEntidadAPI, IServicioSerieDocumental
{
    private DbContextGestionDocumental localContext;
    public ServicioSerieDocumental(DbContextGestionDocumental context, ILogger<ServicioSerieDocumental> logger, IReflectorEntidadesAPI Reflector, IDistributedCache cache) : base(context, context.SerieDocumentales, logger, Reflector, cache)
    {
        interpreteConsulta = new InterpreteConsultaMySQL();
        localContext = context;
    }

    private DbContextGestionDocumental DB { get { return (DbContextGestionDocumental)_db; } }

    public bool RequiereAutenticacion => true;

    public async Task<Respuesta> ActualizarAPI(object id, JsonElement data, StringDictionary? parametros = null)
    {
        var update = data.Deserialize<SerieDocumentalActualizar>(JsonAPIDefaults());
        return await this.Actualizar((string)id, update, parametros);
    }

    public async Task<Respuesta> EliminarAPI(object id, StringDictionary? parametros = null)
    {
        return await this.Eliminar((string)id, parametros);
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
        var add = data.Deserialize<SerieDocumentalInsertar>(JsonAPIDefaults());
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

    #region Overrides para la personalización de la entidad SerieDocumental

    public override async Task<ResultadoValidacion> ValidarInsertar(SerieDocumentalInsertar data)
    {
        ResultadoValidacion resultado = new();
        bool encontrado = await DB.SerieDocumentales.AnyAsync(a => a.Nombre == data.Nombre);

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


    public override async Task<ResultadoValidacion> ValidarEliminacion(string id, SerieDocumental original)
    {
        ResultadoValidacion resultado = new();
        bool encontrado = await DB.SerieDocumentales.AnyAsync(a => a.Id == id);

        if (!encontrado)
        {

            resultado.Error = "id".ErrorProcesoNoEncontrado();

        }
        else
        {
            //bool EncontradoTransferencias = await DB.Transferencias.AnyAsync(a => a.SerieDocumentalId == id);
            //bool EncontradoSerieDocumental = await DB.SerieDocumentales.AnyAsync(a => a.SeriePadreId == id);
            //if (EncontradoSerieDocumental || EncontradoTransferencias)
            //{
            //    resultado.Error = "Id en uso verifique que este no se encuentra en Transferencias O SerieDocumental".Error409();
            //}
            //else
            //{
            //    resultado.Valido = true;
            //}
            resultado.Valido = true;
        }

        return resultado;
    }


    public override async Task<ResultadoValidacion> ValidarActualizar(string id, SerieDocumentalActualizar actualizacion, SerieDocumental original)
    {
        ResultadoValidacion resultado = new();
        bool encontrado = await DB.SerieDocumentales.AnyAsync(a => a.Id == id);

        if (!encontrado)
        {
            resultado.Error = "id".ErrorProcesoNoEncontrado();

        }
        else
        {
            // Verifica que no haya un registro con el mismo nombre para el mismo dominio y UO en un resgitrso diferente
            bool duplicado = await DB.SerieDocumentales.AnyAsync(a => a.Nombre.Equals(actualizacion.Nombre));

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




    public override SerieDocumental ADTOFull(SerieDocumentalActualizar actualizacion, SerieDocumental actual)
    {
        actual.Clave = actualizacion.Clave;
        actual.Nombre = actualizacion.Nombre;
        actual.Raiz = actualizacion.Raiz;
        actual.SeriePadreId = actualizacion.SeriePadreId;
        actual.MesesArchivoTramite = actualizacion.MesesArchivoTramite;
        actual.MesesArchivoConcentracion = actualizacion.MesesArchivoConcentracion;
        actual.MesesArchivoHistorico = actualizacion.MesesArchivoHistorico;
        return actual;
    }


    public override SerieDocumental ADTOFull(SerieDocumentalInsertar data)
    {

        SerieDocumental seriedocumental = new()
        {
            Id = Guid.NewGuid().ToString(),
            CuadroClasificacionId = data.CuadroClasificacionId,
            Clave = data.Clave,
            Nombre = data.Nombre,
            Raiz = data.Raiz,
            SeriePadreId = data.SeriePadreId,
            MesesArchivoTramite = data.MesesArchivoTramite,
            MesesArchivoConcentracion = data.MesesArchivoConcentracion,
            MesesArchivoHistorico = data.MesesArchivoHistorico

        };
        return seriedocumental;
    }


    public override SerieDocumentalDespliegue ADTODespliegue(SerieDocumental data)
    {
        SerieDocumentalDespliegue seriedocumental = new()
        {
            Id = data.Id,
            CuadroClasificacionId = data.CuadroClasificacionId,
            Clave = data.Clave,
            Nombre = data.Nombre,
            Raiz = data.Raiz,
            SeriePadreId = data.SeriePadreId,
            MesesArchivoTramite = data.MesesArchivoTramite,
            MesesArchivoConcentracion = data.MesesArchivoConcentracion,
            MesesArchivoHistorico = data.MesesArchivoHistorico
        };
        return seriedocumental;
    }
    #endregion
}

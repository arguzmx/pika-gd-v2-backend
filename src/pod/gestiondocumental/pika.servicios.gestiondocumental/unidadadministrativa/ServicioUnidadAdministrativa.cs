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

namespace pika.servicios.gestiondocumental.unidadadministrativa;


[ServicioEntidadAPI(entidad: typeof(UnidadAdministrativa))]
public class ServicioUnidadAdministrativa : ServicioEntidadGenericaBase<UnidadAdministrativa, UnidadAdministrativaInsertar, UnidadAdministrativaActualizar, UnidadAdministrativaDespliegue, string>,
    IServicioEntidadAPI, IServicioUnidadAdministrativa
{
    private DbContextGestionDocumental localContext;
    public ServicioUnidadAdministrativa(DbContextGestionDocumental context, ILogger<ServicioUnidadAdministrativa> logger, IReflectorEntidadesAPI Reflector, IDistributedCache cache) : base(context, context.UnidadesAdministrativas, logger, Reflector, cache)
    {
        interpreteConsulta = new InterpreteConsultaMySQL();
        localContext = context;
    }

    private DbContextGestionDocumental DB { get { return (DbContextGestionDocumental)_db; } }

    public bool RequiereAutenticacion => true;

    public async Task<Respuesta> ActualizarAPI(object id, JsonElement data, StringDictionary? parametros = null)
    {
        var update = data.Deserialize<UnidadAdministrativaActualizar>(JsonAPIDefaults());
        return await this.Actualizar((string)id, update, parametros);
    }

    public async Task<Respuesta> EliminarAPI(object id, StringDictionary? parametros = null, bool forzarEliminacion = false)
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
        var add = data.Deserialize<UnidadAdministrativaInsertar>(JsonAPIDefaults());
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

    #region Overrides para la personalización de la entidad UnidadAdministrativa

    public override async Task<ResultadoValidacion> ValidarInsertar(UnidadAdministrativaInsertar data)
    {
        ResultadoValidacion resultado = new();
        bool encontrado = await DB.UnidadesAdministrativas.AnyAsync(a => a.Nombre == data.Nombre);

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


    public override async Task<ResultadoValidacion> ValidarEliminacion(string id, UnidadAdministrativa original, bool forzarEliminacion = false)
    {
        ResultadoValidacion resultado = new();
        bool encontrado = await DB.UnidadesAdministrativas.AnyAsync(a => a.Id == id);

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


    public override Task<Respuesta> EliminarAPI(List<string> ids, StringDictionary? parametros = null, bool forzarEliminacion = false)
    {
        return base.EliminarAPI(ids, parametros, forzarEliminacion);
    }


    public override async Task<ResultadoValidacion> ValidarActualizar(string id, UnidadAdministrativaActualizar actualizacion, UnidadAdministrativa original)
    {
        ResultadoValidacion resultado = new();
        bool encontrado = await DB.UnidadesAdministrativas.AnyAsync(a => a.Id == id);

        if (!encontrado)
        {
            resultado.Error = "id".ErrorProcesoNoEncontrado();

        }
        else
        {
            // Verifica que no haya un registro con el mismo nombre para el mismo dominio y UO en un resgitrso diferente
            bool duplicado = await DB.UnidadesAdministrativas.AnyAsync(a => a.Nombre.Equals(actualizacion.Nombre));

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




    public override UnidadAdministrativa ADTOFull(UnidadAdministrativaActualizar actualizacion, UnidadAdministrativa actual)
    {
        actual.Nombre = actualizacion.Nombre;
        actual.Responsable = actualizacion.Responsable;
        actual.Cargo = actualizacion.Cargo;
        actual.Telefono = actualizacion.Telefono;
        actual.Email = actualizacion.Email;
        actual.Domicilio = actualizacion.Domicilio;
        actual.UbicacionFisica = actualizacion.UbicacionFisica;
        actual.ArchivoTramiteId = actualizacion.ArchivoTramiteId;
        actual.ArchivoConcentracionId = actualizacion.ArchivoConcentracionId;
        actual.ArchivoHistoricoId = actualizacion.ArchivoConcentracionId;
        return actual;
    }


    public override UnidadAdministrativa ADTOFull(UnidadAdministrativaInsertar data)
    {

        UnidadAdministrativa unidadAdministrativa = new()
        {
            Id = Guid.NewGuid().ToString(),
            Nombre = data.Nombre,
            Responsable = data.Responsable,
            Cargo = data.Cargo,
            Telefono = data.Telefono,
            Email = data.Email,
            Domicilio = data.Domicilio,
            UbicacionFisica = data.UbicacionFisica,
            ArchivoTramiteId = data.ArchivoTramiteId,
            ArchivoConcentracionId = data.ArchivoConcentracionId,
            ArchivoHistoricoId = data.ArchivoHistoricoId

        };
        return unidadAdministrativa;
    }


    public override UnidadAdministrativaDespliegue ADTODespliegue(UnidadAdministrativa data)
    {
        UnidadAdministrativaDespliegue unidadAdministrativaDespliegue = new()
        {
            Nombre = data.Nombre,
            Responsable = data.Responsable,
            Cargo = data.Cargo,
            Telefono = data.Telefono,
            Email = data.Email,
            Domicilio = data.Domicilio,
            UbicacionFisica = data.UbicacionFisica,
            ArchivoTramiteId = data.ArchivoTramiteId,
            ArchivoConcentracionId = data.ArchivoConcentracionId,
            ArchivoHistoricoId = data.ArchivoHistoricoId
        };
        return unidadAdministrativaDespliegue;
    }

    #endregion
}

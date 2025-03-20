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

namespace pika.servicios.organizacion.contacto.redsocial
{
    [ServicioEntidadAPI(entidad: typeof(RedSocial))]
    public class ServicioRedSocial : ServicioEntidadGenericaBase<RedSocial, RedSocialInsertar, RedSocialActualizar, RedSocialDespliegue, string>,
        IServicioEntidadAPI, IServicioRedSocial
    {


        private DbContextOrganizacion localContext;

        public ServicioRedSocial(DbContextOrganizacion context, ILogger<ServicioRedSocial> logger, IReflectorEntidadesAPI Reflector, IDistributedCache cache) : base(context, context.RedesSociales, logger, Reflector, cache)
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
            var update = data.Deserialize<RedSocialActualizar>(JsonAPIDefaults());
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
            var add = data.Deserialize<RedSocialInsertar>(JsonAPIDefaults());
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
        #region Overrides para la personalización de la entidad RedSocial

        public override async Task<ResultadoValidacion> ValidarInsertar(RedSocialInsertar data)
        {
            ResultadoValidacion resultado = new();
            bool encontrado = await DB.RedesSociales.AnyAsync(a => a.Url == data.Url && a.UOrgId == _contextoUsuario!.UOrgId && a.DominioId == _contextoUsuario.DominioId);

            if (encontrado)
            {
                resultado.Error = "Url".ErrorProcesoDuplicado();
            }
            else
            {
                resultado.Valido = true;
            }

            return resultado;
        }


        public override async Task<ResultadoValidacion> ValidarEliminacion(string id, RedSocial original, bool forzarEliminacion = false)
        {
            ResultadoValidacion resultado = new();
            bool encontrado = await DB.RedesSociales.AnyAsync(a => a.Id == id && a.UOrgId == _contextoUsuario!.UOrgId && a.DominioId == _contextoUsuario.DominioId);

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


        public override async Task<ResultadoValidacion> ValidarActualizar(string id, RedSocialActualizar actualizacion, RedSocial original)
        {
            ResultadoValidacion resultado = new();

            bool duplicado = await DB.RedesSociales.AnyAsync(a => a.Id != id && a.Url == actualizacion.Url && a.UOrgId == _contextoUsuario!.UOrgId && a.DominioId == _contextoUsuario.DominioId);

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

        // :)
        public override RedSocial ADTOFull(RedSocialActualizar actualizacion, RedSocial actual)
        {
            actual.Id = actualizacion.Id;
            actual.Url = actualizacion.Url;
            actual.TipoRedSocialId = actualizacion.TipoRedSocialId;
            return actual;
        }

        public override RedSocial ADTOFull(RedSocialInsertar data)
        {
            RedSocial contenido = new()
            {
                Id = Guid.NewGuid().ToString(),
                Url = data.Url,
                TipoRedSocialId = data.TipoRedSocialId,
                DominioId = _contextoUsuario.DominioId,
                UOrgId = _contextoUsuario.UOrgId


            };
            return contenido;
        }

        public override RedSocialDespliegue ADTODespliegue(RedSocial data)
        {
            RedSocialDespliegue redSocialDespliegue = new()
            {
                Id = data.Id,
                Url = data.Url
            };
            return redSocialDespliegue;
        }

        #endregion
    }
}

using apigenerica.model.modelos;
using apigenerica.model.reflectores;
using apigenerica.model.servicios;
using comunes.primitivas;
using Microsoft.Extensions.Logging;
using pika.modelo.contenido;
using pika.servicios.contenido.dbcontext;

namespace pika.servicios.contenido.volumen;
//[ServicioCatalogoEntidadAPI(typeof(TipoGestorES))]
//public class ServicioTipoGestorES : ServicioCatalogoGenericoBase, IServicioCatalogoAPI, IServicioTipoGestorES
//{
//    public ServicioTipoGestorES(ILogger<ServicioTipoGestorES> logger, DbContextContenido context) : base(context, context.TipoGestorES, logger) 
//    {
//    }
//    public bool RequiereAutenticacion => true;

//    public string IdiomaDefault => "es-MX";

//    public List<ElementoCatalogo> ElementosDefault()
//    {
//        return new List<ElementoCatalogo>()
//        {

//        };
//    }

//    #region Override

//    private DbContextContenido DB { get { return (DbContextContenido)_db; } }

//    public override async Task<ResultadoValidacion> ValidarEliminacion(string id, ElementoCatalogo original)
//    {
//        ResultadoValidacion resultado = new();

//        resultado.Valido = true;
//        // Verifica que el Id no esté en uso en la tabla de Volumenes
//        if (DB.Volumenes.Any(
//            a => a.TipoGestorESId == id
//            && a.DominioId == _contextoUsuario.DominioId
//            && a.UOrgId == _contextoUsuario.UOrgId))
//        {
//            resultado.Valido = false;
//        }

//        return resultado;
//    }

//    #endregion

//}

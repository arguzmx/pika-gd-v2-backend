using apigenerica.model.servicios;
using pika.modelo.organizacion;

namespace pika.servicios.organizacion.usuariodominio;

public interface IServicioUsuarioDominio : IServicioEntidadGenerica<UsuarioDominio,UsuarioDominioInsertar,UsuarioDominioActualizar,UsuarioDominioDespliegue, string>
{
}

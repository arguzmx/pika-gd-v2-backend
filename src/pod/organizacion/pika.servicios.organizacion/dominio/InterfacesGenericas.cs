using apigenerica.model.servicios;
using pika.modelo.organizacion;

namespace pika.servicios.organizacion.dominio;

public interface IServicioDominio : IServicioEntidadGenerica<Dominio,DominioInsertar,DominioActualizar,DominioDespliegue,string>
{ 
}
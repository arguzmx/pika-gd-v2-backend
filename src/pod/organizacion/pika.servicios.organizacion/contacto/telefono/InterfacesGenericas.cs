using apigenerica.model.servicios;
using pika.modelo.organizacion;
using pika.modelo.organizacion.Contacto;

namespace pika.servicios.organizacion.contacto.telefono;

public interface IServicioTelefono : IServicioEntidadGenerica<Telefono, TelefonoInsertar, TelefonoActualizar, TelefonoDespliegue, string>
{
}

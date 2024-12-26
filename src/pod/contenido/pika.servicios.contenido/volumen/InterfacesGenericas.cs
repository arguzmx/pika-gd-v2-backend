using apigenerica.model.reflectores;
using apigenerica.model.servicios;
using pika.modelo.contenido;

namespace pika.servicios.contenido.volumen;


/// <summary>
/// Interface servicio volumen
/// </summary>
public interface IServicioVolumen : IServicioEntidadGenerica<Volumen, VolumenInsertar, VolumenActualizar, VolumenDespliegue, string>
{
}

public interface IServicioTipoGestorES : IServicioCatalogoAPI
{ }


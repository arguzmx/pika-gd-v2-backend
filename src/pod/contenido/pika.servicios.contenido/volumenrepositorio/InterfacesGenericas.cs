using apigenerica.model.servicios;
using comunes.primitivas;
using Microsoft.AspNetCore.Http;
using pika.modelo.contenido;
using System.Collections.Specialized;

namespace pika.servicios.contenido.volumenrepositorio;

public interface IServicioVolumenRepositorio : IServicioEntidadGenerica<VolumenRepositorio, VolumenRepositorioCrear, VolumenRepositorioActualizar, VolumenRepositorio, string>
{
    public StringDictionary ParametrosRuta(HttpContext context);
    public Task<RespuestaPayload<List<Volumen>>> PaginaVolumenes(string repoId);
    Task<RespuestaPayload<VolumenDespliegue>> Insertar(VolumenRepositorioCrear data, StringDictionary? parametros = null);
}

namespace pika.modelo.contenido;

public class CarpetaDespliegue
{
    public string Id { get; set; }
    public string CreadorId { get; set; }
    public DateTime FechaCreacion { get; set; }
    public string Nombre { get; set; }
    public string? CarpetaPadreId { get; set; }
}


namespace pika.modelo.contenido;
public class VersionDespliegue
{
    public string Id { get; set; }
    public string Rev { get; set; }
    public string ContenidoId { get; set; }
    public bool Activa { get; set; } = true;
    public DateTime FechaCreacion { get; set; }
    public string CreadorId { get; set; }
    public int ConteoPartes { get; set; } = 0;
    public long TamanoBytes { get; set; } = 0;
    public string VolumenId { get; set; }
}

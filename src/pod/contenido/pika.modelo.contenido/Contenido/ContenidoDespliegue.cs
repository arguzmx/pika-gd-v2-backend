namespace pika.modelo.contenido;

public class ContenidoDespliegue
{
    public string Id { get; set; }
    public string Nombre { get; set; }
    public string CreadorId { get; set; }
    public DateTime FechaCreacion { get; set; }
    public int ConteoAnexos { get; set; } = 0;
    public long TamanoBytes { get; set; } = 0;
    public string VolumenId { get; set; }
    public string CarpetaId { get; set; }
    public string? TipoElemento { get; set; }
    public string? IdExterno { get; set; }
}
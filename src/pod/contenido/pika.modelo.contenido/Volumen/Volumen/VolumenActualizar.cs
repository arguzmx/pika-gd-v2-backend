namespace pika.modelo.contenido;

public class VolumenActualizar
{
    public string Id { get; set; }
    public string Nombre { get; set; }
    public string TipoGestorESId { get; set; }
    public long TamanoMaximo { get; set; }
    public bool Activo { get; set; }
    public bool EscrituraHabilitada { get; set; }
}

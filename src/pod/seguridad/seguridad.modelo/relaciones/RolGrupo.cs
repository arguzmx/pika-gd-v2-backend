using seguridad.modelo.instancias;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace seguridad.modelo.relaciones;

public class RolGrupo
{
    public string Id { get; set; }
    public required string RolId { get; set; }
    public required Guid GrupoId { get; set; }

    [NotMapped]
    [JsonIgnore]
    public InstanciaAplicacion InstanciaAplicacion { get; set; }

    [NotMapped]
    [JsonIgnore]
    public GrupoUsuarios Grupo { get; set; }

    [NotMapped]
    [JsonIgnore]
    public Rol Rol { get; set; }
}

using seguridad.modelo.instancias;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace seguridad.modelo.relaciones;

public class PermisoGrupo
{
    public required string Id { get; set; }
    public required string PermisoId { get; set; }
    public required Guid GrupoId { get; set; }

    [NotMapped]
    [JsonIgnore]
    public InstanciaAplicacion InstanciaAplicacion { get; set; }

    [NotMapped]
    [JsonIgnore]
    public GrupoUsuarios Grupo { get; set; }

    [NotMapped]
    [JsonIgnore]
    public Permiso Permiso { get; set; }
}

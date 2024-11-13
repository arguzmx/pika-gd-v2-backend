using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pika.modelo.organizacion
{
    public class UsuarioUnidadOrganizacionalDespliegue
    {
        /// <summary>
        /// Identificador único de la relacióm
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Identificador único del usuario, este viene del servicio de identidad
        /// </summary>
        public string UsuarioId { get; set; }

        /// <summary>
        /// Identificador único del dominio
        /// </summary>
        public string DominioId { get; set; }

        /// <summary>
        /// Identificador único del dominio
        /// </summary>
        public string UnidadOrganizacionalId { get; set; }


    }
}

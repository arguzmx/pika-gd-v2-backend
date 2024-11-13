using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using pika.modelo.organizacion;

namespace pika.api.organizacion.Controllers
{
    [Route("api/internal")]
    [ApiController]
    [Authorize("InternalScope")]
    public class InternalController : ControllerBase
    {

        public InternalController() {
        }


        [HttpPost("{usuarioId}/dominio")]
        public async Task<ActionResult<DominioDespliegue>> CreaDominioUsuario([FromBody] DominioInsertar dominio, [FromRoute] string usuarioId)
        {

            return Ok();
        }


    }
}

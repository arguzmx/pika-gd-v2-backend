﻿using apigenerica.primitivas;

namespace pika.api.organizacion.Controllers;

public class EntidadGenericaN2Controller : ControladorGenericoN2
{
    private ILogger<EntidadGenericaN2Controller> _logger;

    public EntidadGenericaN2Controller(ILogger<EntidadGenericaN2Controller> logger, IHttpContextAccessor httpContextAccessor) : base(logger, httpContextAccessor)
    {
        this._logger = logger;
    }
}

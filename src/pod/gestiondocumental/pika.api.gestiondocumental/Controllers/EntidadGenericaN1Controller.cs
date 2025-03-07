﻿using apigenerica.primitivas;

namespace pika.api.gestiondocumental.Controllers;


public class EntidadGenericaN1Controller : ControladorGenericoN1
{
    private ILogger<EntidadGenericaN1Controller> _logger;

    public EntidadGenericaN1Controller(ILogger<EntidadGenericaN1Controller> logger, IHttpContextAccessor httpContextAccessor) : base(logger, httpContextAccessor)
    {
        this._logger = logger;
    }
}
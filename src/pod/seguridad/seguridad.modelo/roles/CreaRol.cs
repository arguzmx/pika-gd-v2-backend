﻿using MongoDB.Bson.Serialization.Attributes;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace seguridad.modelo.roles;

[ExcludeFromCodeCoverage]
public class CreaRol
{
    /// <summary>
    /// Nombre del rol para la UI, esto será calcolado en base al idioma o bien al crear roles personalizados
    /// </summary>
    public required string Nombre { get; set; }

    /// <summary>
    /// Descripción del rol para la UI, esto será calcolado en base al idioma o bien al crear roles personalizados
    /// </summary>
    public string? Descripcion { get; set; }

    [BsonIgnore]
    public string? InstanciaAplicacionId { get; set; }

    [BsonIgnore]
    public string? ModuloId { get; set; }
}
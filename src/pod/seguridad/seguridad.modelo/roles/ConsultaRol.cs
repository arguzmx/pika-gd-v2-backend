﻿using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace seguridad.modelo.roles;

[ExcludeFromCodeCoverage]
public class ConsultaRol
{
    /// <summary>
    /// Identificador único del rol, se utiliza como clave para los roles y para la i18N, debe ser único en la lista de permisos de una app
    /// </summary>
    public required string RolId { get; set; }

    /// <summary>
    /// Lista de los identificadores de permisos asociados al rol
    /// </summary>
    public List<string> Permisos { get; set; } = [];

    /// <summary>
    /// DEfine si un rol ha sido creado por el administrador de sistema
    /// </summary>
    public bool Personalizado { get; set; }

    /// <summary>
    /// Nombre del rol para la UI, esto será calcolado en base al idioma o bien al crear roles personalizados
    /// </summary>
    public string? Nombre { get; set; }

    /// <summary>
    /// Descripción del rol para la UI, esto será calcolado en base al idioma o bien al crear roles personalizados
    /// </summary>
    public string? Descripcion { get; set; }

    [BsonIgnore]
    public string? InstanciaAplicacionId { get; set; }

    [BsonIgnore]
    public string ModuloId { get; set; }
}

using extensibilidad.metadatos.atributos;
using extensibilidad.metadatos.validadores;
using extensibilidad.metadatos;

namespace pika.modelo.contenido;


//[Entidad(nameof(DemoMetadatos))]
//public class DemoMetadatos
//{
//    [Id]
//    [Propiedad(tipoDato: TipoDatos.Texto)]
//    [Formulario(indice: 1, visible: false, tipoDespliegue: TipoDespliegue.Oculto)]
//    [Tabla(indice: 1, visible: false, ancho: 10)]
//    [ValidarRequerida(requerida: RequeridaOperacion.Actualizar)]
//    public string Id { get; set; }

//    [Propiedad(tipoDato: TipoDatos.Texto)]
//    [Formulario(indice: 2, tipoDespliegue: TipoDespliegue.TextoCorto)]
//    [Tabla(indice: 2, ancho: 10)]
//    [ValidarRequerida(requerida: RequeridaOperacion.Insertar), ValidarRequerida(requerida: RequeridaOperacion.Actualizar)]
//    [ValidarTexto(longitudMinima: "2")]
//    public string Nombre { get; set; }


//    [Propiedad(tipoDato: TipoDatos.Entero, valorDefault: "1")]
//    [Formulario(indice: 3, tipoDespliegue: TipoDespliegue.SliderNumerico)]
//    [Tabla(indice: 3, ancho: 10)]
//    [ValidarEntero(minimo: "0", maximo: "100")]
//    public int Experiencia { get; set; }


//    [Propiedad(tipoDato: TipoDatos.Decimal)]
//    [Formulario(indice: 4, tipoDespliegue: TipoDespliegue.TextoNumerico)]
//    [Tabla(indice: 4, ancho: 10)]
//    [ValidarDecimal(minimo: "0")]
//    [ValidarRequerida(requerida: RequeridaOperacion.Insertar), ValidarRequerida(requerida: RequeridaOperacion.Actualizar)]
//    public decimal PrecioPorHora { get; set; }



//    [Propiedad(tipoDato: TipoDatos.TextoIndexado)]
//    [Formulario(indice: 5, tipoDespliegue: TipoDespliegue.TextoLargo)]
//    [Tabla(indice: 5, ancho: 30)]
//    public string Curriculum { get; set; }


//    [Propiedad(tipoDato: TipoDatos.Fecha, valorDefault: "2000-02-02T00:00:00")]
//    [Formulario(indice: 6, tipoDespliegue: TipoDespliegue.TextoFecha)]
//    [Tabla(indice: 6, ancho: 10)]
//    [ValidarFecha(minimo: "1/1/1950", tipo: TipoDatos.Fecha, formato: ValidarFechaAttribute.FECHA_DMY)]
//    public DateTime FechaNacimiento { get; set; }


//    [Propiedad(tipoDato: TipoDatos.Hora, buscable: false, valorDefault: "1900-01-01T12:00:00")]
//    [Formulario(indice: 7, tipoDespliegue: TipoDespliegue.TextoHora)]
//    [Tabla(indice: 7, ancho: 10)]
//    [ValidarFecha(minimo: "10:00:00", maximo: "16:30:00", tipo: TipoDatos.Hora, formato: ValidarFechaAttribute.HORA_HMS24)]
//    [ValidarRequerida(requerida: RequeridaOperacion.Actualizar)]
//    public DateOnly HoraDeLunch { get; set; }



//    [Propiedad(tipoDato: TipoDatos.FechaHora, valorDefault: "2020-01-01T08:00:00")]
//    [Formulario(indice: 8, tipoDespliegue: TipoDespliegue.TextoFechaHora)]
//    [Tabla(indice: 8, ancho: 10)]
//    [ValidarFecha(minimo: "2024-01-01T00:00:00", maximo: "2024-12-31T23:59:59", tipo: TipoDatos.FechaHora, formato: ValidarFechaAttribute.FECHA_HORA24_ISO)]
//    [ValidarRequerida(requerida: RequeridaOperacion.Insertar)]
//    public DateTime FechaCreacion { get; set; }

//    [Propiedad(tipoDato: TipoDatos.Logico, valorDefault: "true")]
//    [Formulario(indice: 9, tipoDespliegue: TipoDespliegue.Switch)]
//    [Tabla(indice: 9, ancho: 10, alternable: false, visible: true)]
//    [ValidarRequerida(requerida: RequeridaOperacion.Insertar)]
//    public bool Activo { get; set; }


//    [Propiedad(tipoDato: TipoDatos.ListaSeleccionSimple, valorDefault: "Generos-2")]
//    [Formulario(indice: 10, tipoDespliegue: TipoDespliegue.ListaSelecciónSimple)]
//    [Tabla(indice: 10, ancho: 10)]
//    [ListaAtttribute(remota: false, claveLocal: "Generos", seleccionMinima: 1)]
//    public string Genero { get; set; }


//    [Propiedad(tipoDato: TipoDatos.ListaSeleccionMultiple, valorDefault: "Especialidades-1,Especialidades-3")]
//    [Formulario(indice: 11, tipoDespliegue: TipoDespliegue.ListaSeleccionMultiple)]
//    [Tabla(indice: 11, ancho: 10)]
//    [ListaAtttribute(remota: false, claveLocal: "Especialidades", seleccionMinima: 2)]
//    public List<string> Especialidades { get; set; } = [];
//}

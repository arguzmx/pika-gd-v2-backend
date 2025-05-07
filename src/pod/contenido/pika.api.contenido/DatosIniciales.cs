using CouchDB.Driver.Extensions;
using pika.modelo.contenido;
using pika.servicios.contenido.dbcontext;

namespace pika.api.contenido
{
    public static class DatosIniciales
    {
        public const string IDGENERICO = "10000000-0000-0000-0000-000000000001";
        public const string IDGENERICO2 = "10000000-0000-0000-0000-000000000002";
        public const string IDGENERICO3 = "10000000-0000-0000-0000-000000000003";


        public static async Task CatalogoGenericos(this WebApplication app)
        {
            using var scope = app.Services.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<DbContextContenido>();
            var listElementos = db.ElementosCatalogo.OfType<TipoGestorES>().Where(e => e.CatalogoId == "FSLOCAL" && e.DominioId == Guid.Empty.ToString()).ToList();

            if (listElementos.Count == 0)
            {
                TipoGestorES ec = new TipoGestorES()
                {
                    Id = IDGENERICO,
                    Idioma = "es",
                    Texto = "Gestor de archivos filesystem local",
                    DominioId = Guid.Empty.ToString(),
                    CatalogoId = "FSLOCAL",
                    UnidadOrganizacionalId = Guid.Empty.ToString()
                };
                db.ElementosCatalogo.Add(ec);
                db.SaveChanges();
            }

            listElementos = db.ElementosCatalogo.OfType<TipoGestorES>().Where(e => e.CatalogoId == "BUCKETGCP" && e.DominioId == Guid.Empty.ToString()).ToList();
            if (listElementos.Count == 0)
            {
                TipoGestorES ec = new TipoGestorES()
                {
                    Id = IDGENERICO2,
                    Idioma = "es",
                    Texto = "Gestor de archivos bucket GCP",
                    DominioId = Guid.Empty.ToString(),
                    CatalogoId = "BUCKETGCP",
                    UnidadOrganizacionalId = Guid.Empty.ToString()
                };
                db.ElementosCatalogo.Add(ec);
                db.SaveChanges();
            }
        }

        public static async Task DummyData(this WebApplication app)
        {
            using var scope = app.Services.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<DbContextContenido>();

            var listaVol = db.Volumenes.Where(v => v.Id == IDGENERICO && v.DominioId == IDGENERICO && v.UOrgId == IDGENERICO);
            if (listaVol.Count() == 0)
            {
                Volumen volumen = new Volumen()
                {
                    Id = IDGENERICO,
                    Nombre = "Volumen de prueba",
                    Activo = true,
                    DominioId = IDGENERICO,
                    UOrgId = IDGENERICO,
                    ConfiguracionValida = true,
                     TipoGestorESId  = IDGENERICO,
                };
                db.Volumenes.Add(volumen);
                await db.SaveChangesAsync();
            }

 

        }
    }
}

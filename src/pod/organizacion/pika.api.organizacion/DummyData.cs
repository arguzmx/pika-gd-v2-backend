using pika.modelo.organizacion;
using pika.servicios.organizacion.dbcontext;

namespace pika.api.organizacion
{
    public static class DatosIniciales
    {
        public const string IDGENERICO = "10000000-0000-0000-0000-000000000001";
        public const string IDGENERICO2 = "10000000-0000-0000-0000-000000000002";
        public const string IDGENERICO3 = "10000000-0000-0000-0000-000000000003";
        public static async Task DummyData(this WebApplication app)
        {
            using var scope = app.Services.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<DbContextOrganizacion>();
            await CreaDominioUsuario(db, IDGENERICO, IDGENERICO, IDGENERICO, IDGENERICO3);
            await CreaDominioUsuario(db, IDGENERICO2, IDGENERICO2, IDGENERICO);
        }

        private async static Task CreaDominioUsuario(DbContextOrganizacion db,string dominioId, string ouId, string userId, string? ouId2 = null )
        {
#pragma warning disable CS8600 // Se va a convertir un literal nulo o un posible valor nulo en un tipo que no acepta valores NULL
            Dominio d = db.Dominios.FirstOrDefault(d => d.Id == dominioId);
            if (d == null)
            {
                d = new Dominio()
                {
                    Id = dominioId,
                    Nombre = $"Dominio de prueba {dominioId.Split('-')[4]}",
                    FechaCreacion = DateTime.UtcNow,
                    Activo = true,
                    UsuarioId = userId
                };
                db.Dominios.Add(d);
                await db.SaveChangesAsync();
            }

            UnidadOrganizacional u = db.UnidadesOrganizacionales.FirstOrDefault(u => u.Id == ouId);
            if (u == null)
            {
                u = new UnidadOrganizacional()
                {
                    Id = ouId,
                    Nombre = $"Unidad de prueba {ouId.Split('-')[4]}",
                    DominioId = dominioId,
                };
                db.UnidadesOrganizacionales.Add(u);
                await db.SaveChangesAsync();
            }

            UsuarioDominio usuarioDominio = db.UsuarioDominios.FirstOrDefault(u => u.UsuarioId == userId && u.DominioId == dominioId);
            if (usuarioDominio == null)
            {
                usuarioDominio = new UsuarioDominio()
                {
                    Id = dominioId,
                    DominioId = dominioId,
                    UsuarioId = userId,
                };
                db.UsuarioDominios.Add(usuarioDominio);
                await db.SaveChangesAsync();
            }

            UsuarioUnidadOrganizacional usuarioUnidadOrganizacional = db.UsuariosUnidadesOrganizacionales.FirstOrDefault(u => u.UsuarioId == userId && u.UnidadOrganizacionalId == ouId);
            if (usuarioUnidadOrganizacional == null)
            {
                usuarioUnidadOrganizacional = new UsuarioUnidadOrganizacional()
                {
                    Id = ouId,
                    DominioId = dominioId,
                    UsuarioId = userId,
                    UnidadOrganizacionalId = ouId
                };
                db.UsuariosUnidadesOrganizacionales.Add(usuarioUnidadOrganizacional);
                await db.SaveChangesAsync();
            }


            if(ouId2!=null)
            {
                var u2 = db.UnidadesOrganizacionales.FirstOrDefault(u => u.Id == ouId2);
                if (u2 == null)
                {
                    u2 = new UnidadOrganizacional()
                    {
                        Id = ouId2,
                        Nombre = $"Unidad de prueba {ouId2.Split('-')[4]}",
                        DominioId = dominioId,
                    };
                    db.UnidadesOrganizacionales.Add(u2);
                    await db.SaveChangesAsync();
                }

                var usuarioUnidadOrganizacional2 = db.UsuariosUnidadesOrganizacionales.FirstOrDefault(u => u.UsuarioId == userId && u.UnidadOrganizacionalId == ouId2);
                if (usuarioUnidadOrganizacional2 == null)
                {
                    usuarioUnidadOrganizacional2 = new UsuarioUnidadOrganizacional()
                    {
                        Id = ouId2,
                        DominioId = dominioId,
                        UsuarioId = userId,
                        UnidadOrganizacionalId = ouId2
                    };
                    db.UsuariosUnidadesOrganizacionales.Add(usuarioUnidadOrganizacional2);
                    await db.SaveChangesAsync();
                }
            }

#pragma warning restore CS8600 // Se va a convertir un literal nulo o un posible valor nulo en un tipo que no acepta valores NULL
        }
    }
}

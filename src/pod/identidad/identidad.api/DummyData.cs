using contabee.identity.api.models;
using Microsoft.AspNetCore.Identity;

namespace identidad.api
{
    public static class DatosIniciales
    {
        public const string ADMINISTRADOR = "10000000-0000-0000-0000-000000000001";

        public static async Task DummyData(this WebApplication app)
        {
            using var scope = app.Services.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            var users = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var roles = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            var rol = await roles.FindByIdAsync("SYSADMIN");
            if(rol == null)
            {
                IdentityRole role = new IdentityRole()
                {
                    Name = "SYSADMIN",
                    NormalizedName = "SYSADMIN", 
                    Id = "SYSADMIN",
                };
                await roles.CreateAsync(role);
            }   


#pragma warning disable CS8600 // Se va a convertir un literal nulo o un posible valor nulo en un tipo que no acepta valores NULL
            ApplicationUser user = await users.FindByIdAsync(ADMINISTRADOR);
#pragma warning restore CS8600 // Se va a convertir un literal nulo o un posible valor nulo en un tipo que no acepta valores NULL
            if (user == null)
            {
                ApplicationUser adminUser = new ()
                {
                    Email = "admin@interno.com",
                    UserName = "admin@interno.com",
                    EmailConfirmed = true,
                    Id = ADMINISTRADOR,
                    Estado = EstadoCuenta.Activo,
                    FechaActivacion = DateTime.UtcNow,
                    FechaRegistro = DateTime.UtcNow
                };

                IdentityResult result = await users.CreateAsync(adminUser, "Pa$$w0rd");

                if(result.Succeeded)
                {
                    users.AddToRoleAsync(adminUser, "SYSADMIN").Wait();  
                }
            }
        }
    }
}

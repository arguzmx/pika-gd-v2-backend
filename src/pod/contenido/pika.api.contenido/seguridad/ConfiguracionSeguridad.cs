using apigenerica.primitivas.aplicacion;
using comunes.primitivas.seguridad;

namespace pika.api.contenido.seguridad;

public class ConfiguracionSeguridad : IProveedorAplicaciones
{
    public Task<List<Aplicacion>> ObtieneApliaciones()
    {
        List<Aplicacion> apps = [];

        apps.Add(
            new Aplicacion
            {
                ApplicacionId = Guid.Parse(ConfiguracionSeguridadConstantes.AplicacionId),
                Nombre = "Gestor de Contenidos",
                Descripcion = "Gestor de Contenidos de la solución",
                Modulos = [
                 new Modulo()
                 {
                     Nombre = "Administrador de Contenidos",
                     Descripcion = "Permite administrar las Contenidos de la solución",
                     ModuloId = "org-manager",
                     RolesPredefinidos = [
                             new()
                             {
                                 Nombre = "Administrador",
                                 Descripcion = "Todos los permisos para la administración de contenidos",
                                 Permisos = [ConfiguracionSeguridadConstantes.CONT_CARPETA_PERM_ADMIN],
                                 Personalizado = false,
                                 RolId = ConfiguracionSeguridadConstantes.CONT_CARPETA_ROL_ADMIN
                             }

                         ],
                     Permisos = [
                             new()
                             {
                                 Nombre = "Administrar Organizaciones",
                                 Descripcion = "Permite crear, editar y eliminar Contenidos",
                                 Ambito = AmbitoPermiso.Global,
                                 PermisoId = ConfiguracionSeguridadConstantes.CONT_CARPETA_PERM_ADMIN
                             },
                         new()
                         {
                             Nombre = "Listar carpetas",
                             Descripcion = "Permite obtener la lista de carpetas",
                             Ambito = AmbitoPermiso.Global,
                             PermisoId = ConfiguracionSeguridadConstantes.CONT_CARPETA_PERM_LIST
                         }
                         ]
                 },
                ]

            });
        return Task.FromResult(apps);
    }
}

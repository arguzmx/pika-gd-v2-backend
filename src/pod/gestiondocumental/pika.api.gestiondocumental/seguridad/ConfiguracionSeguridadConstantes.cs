namespace pika.api.gestiondocumental.seguridad;

public class ConfiguracionSeguridadConstantes
{
    public const string AplicacionId = "00000000-0000-0000-0000-000000000001";
    #region Permisos

    /// <summary>
    /// Permite crear, editar y eliminar dominio
    /// </summary>
    public const string CONT_CARPETA_PERM_ADMIN = "cont-carpeta-perm-admin";
    /// <summary>
    /// Permite crear, editar y eliminar dominio
    /// </summary>
    public const string CONT_CARPETA_PERM_LIST = "cont-carpeta-perm-list";

    #endregion


    #region Roles 

    /// <summary>
    /// Tiene todos los permisos para administrar dominios
    /// </summary>
    public const string CONT_CARPETA_ROL_ADMIN = "cont-carpeta-rol-admin";

    #endregion

}

using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace pika.servicios.contenido.data.migrations
{
    /// <inheritdoc />
    public partial class InicialContenido : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "cont$catalogos",
                columns: table => new
                {
                    Id = table.Column<string>(type: "varchar(128)", maxLength: 128, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Idioma = table.Column<string>(type: "varchar(10)", maxLength: 10, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Texto = table.Column<string>(type: "varchar(512)", maxLength: 512, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DominioId = table.Column<string>(type: "varchar(128)", maxLength: 128, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    UnidadOrganizacionalId = table.Column<string>(type: "varchar(128)", maxLength: 128, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CatalogoId = table.Column<string>(type: "varchar(128)", maxLength: 128, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Discriminator = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_cont$catalogos", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "cont$i18ncatalogos",
                columns: table => new
                {
                    Id = table.Column<string>(type: "varchar(128)", maxLength: 128, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Idioma = table.Column<string>(type: "varchar(10)", maxLength: 10, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DominioId = table.Column<string>(type: "varchar(128)", maxLength: 128, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    UnidadOrganizacionalId = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Texto = table.Column<string>(type: "varchar(512)", maxLength: 512, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CatalogoId = table.Column<string>(type: "varchar(128)", maxLength: 128, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Discriminator = table.Column<int>(type: "int", nullable: false),
                    ElementoCatalogoId = table.Column<string>(type: "varchar(128)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_cont$i18ncatalogos", x => new { x.Id, x.DominioId, x.UnidadOrganizacionalId, x.Idioma });
                    table.ForeignKey(
                        name: "FK_cont$i18ncatalogos_cont$catalogos_ElementoCatalogoId",
                        column: x => x.ElementoCatalogoId,
                        principalTable: "cont$catalogos",
                        principalColumn: "Id");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "cont$volumen",
                columns: table => new
                {
                    Id = table.Column<string>(type: "varchar(128)", maxLength: 128, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DominioId = table.Column<string>(type: "varchar(128)", maxLength: 128, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    UOrgId = table.Column<string>(type: "varchar(128)", maxLength: 128, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Nombre = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    TipoGestorESId = table.Column<string>(type: "varchar(128)", maxLength: 128, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    TamanoMaximo = table.Column<long>(type: "bigint", nullable: false),
                    Activo = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    EscrituraHabilitada = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    ConsecutivoVolumen = table.Column<long>(type: "bigint", nullable: false),
                    CanidadPartes = table.Column<long>(type: "bigint", nullable: false),
                    CanidadElementos = table.Column<long>(type: "bigint", nullable: false),
                    Tamano = table.Column<long>(type: "bigint", nullable: false),
                    ConfiguracionValida = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_cont$volumen", x => x.Id);
                    table.ForeignKey(
                        name: "FK_cont$volumen_cont$catalogos_TipoGestorESId",
                        column: x => x.TipoGestorESId,
                        principalTable: "cont$catalogos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "cont$repositorio",
                columns: table => new
                {
                    Id = table.Column<string>(type: "varchar(128)", maxLength: 128, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DominioId = table.Column<string>(type: "varchar(128)", maxLength: 128, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    UOrgId = table.Column<string>(type: "varchar(128)", maxLength: 128, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Nombre = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    VolumenId = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_cont$repositorio", x => x.Id);
                    table.ForeignKey(
                        name: "FK_cont$repositorio_cont$volumen_VolumenId",
                        column: x => x.VolumenId,
                        principalTable: "cont$volumen",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "cont$carpeta",
                columns: table => new
                {
                    Id = table.Column<string>(type: "varchar(128)", maxLength: 128, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    RepositorioId = table.Column<string>(type: "varchar(128)", maxLength: 128, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreadorId = table.Column<string>(type: "varchar(128)", maxLength: 128, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    FechaCreacion = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Nombre = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CarpetaPadreId = table.Column<string>(type: "varchar(128)", maxLength: 128, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    EsRaiz = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    PermisoId = table.Column<string>(type: "varchar(128)", maxLength: 128, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_cont$carpeta", x => x.Id);
                    table.ForeignKey(
                        name: "FK_cont$carpeta_cont$repositorio_RepositorioId",
                        column: x => x.RepositorioId,
                        principalTable: "cont$repositorio",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "cont$contenido",
                columns: table => new
                {
                    Id = table.Column<string>(type: "varchar(128)", maxLength: 128, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Nombre = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    RepositorioId = table.Column<string>(type: "varchar(128)", maxLength: 128, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreadorId = table.Column<string>(type: "varchar(128)", maxLength: 128, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    FechaCreacion = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    ConteoAnexos = table.Column<int>(type: "int", nullable: false),
                    TamanoBytes = table.Column<long>(type: "bigint", nullable: false),
                    VolumenId = table.Column<string>(type: "varchar(128)", maxLength: 128, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CarpetaId = table.Column<string>(type: "varchar(128)", maxLength: 128, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    TipoElemento = table.Column<string>(type: "varchar(128)", maxLength: 128, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    IdExterno = table.Column<string>(type: "varchar(128)", maxLength: 128, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    PermisoId = table.Column<string>(type: "varchar(128)", maxLength: 128, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_cont$contenido", x => x.Id);
                    table.ForeignKey(
                        name: "FK_cont$contenido_cont$carpeta_CarpetaId",
                        column: x => x.CarpetaId,
                        principalTable: "cont$carpeta",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_cont$contenido_cont$repositorio_RepositorioId",
                        column: x => x.RepositorioId,
                        principalTable: "cont$repositorio",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_cont$contenido_cont$volumen_VolumenId",
                        column: x => x.VolumenId,
                        principalTable: "cont$volumen",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_cont$carpeta_RepositorioId",
                table: "cont$carpeta",
                column: "RepositorioId");

            migrationBuilder.CreateIndex(
                name: "IX_cont$catalogos_CatalogoId",
                table: "cont$catalogos",
                column: "CatalogoId");

            migrationBuilder.CreateIndex(
                name: "IX_cont$contenido_CarpetaId",
                table: "cont$contenido",
                column: "CarpetaId");

            migrationBuilder.CreateIndex(
                name: "IX_cont$contenido_RepositorioId",
                table: "cont$contenido",
                column: "RepositorioId");

            migrationBuilder.CreateIndex(
                name: "IX_cont$contenido_VolumenId",
                table: "cont$contenido",
                column: "VolumenId");

            migrationBuilder.CreateIndex(
                name: "IX_cont$i18ncatalogos_CatalogoId_Idioma",
                table: "cont$i18ncatalogos",
                columns: new[] { "CatalogoId", "Idioma" });

            migrationBuilder.CreateIndex(
                name: "IX_cont$i18ncatalogos_ElementoCatalogoId",
                table: "cont$i18ncatalogos",
                column: "ElementoCatalogoId");

            migrationBuilder.CreateIndex(
                name: "IX_cont$repositorio_VolumenId",
                table: "cont$repositorio",
                column: "VolumenId");

            migrationBuilder.CreateIndex(
                name: "IX_cont$volumen_TipoGestorESId",
                table: "cont$volumen",
                column: "TipoGestorESId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "cont$contenido");

            migrationBuilder.DropTable(
                name: "cont$i18ncatalogos");

            migrationBuilder.DropTable(
                name: "cont$carpeta");

            migrationBuilder.DropTable(
                name: "cont$repositorio");

            migrationBuilder.DropTable(
                name: "cont$volumen");

            migrationBuilder.DropTable(
                name: "cont$catalogos");
        }
    }
}

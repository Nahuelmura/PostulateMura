using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Postulate.Migrations
{
    /// <inheritdoc />
    public partial class cambiosimagen : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Servicios_Imagen_ImagenID",
                table: "Servicios");

            migrationBuilder.DropIndex(
                name: "IX_Servicios_ImagenID",
                table: "Servicios");

            migrationBuilder.DropColumn(
                name: "ImagenID",
                table: "Servicios");

            migrationBuilder.AddColumn<int>(
                name: "ServicioID",
                table: "Imagen",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ServicioID",
                table: "Imagen");

            migrationBuilder.AddColumn<int>(
                name: "ImagenID",
                table: "Servicios",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Servicios_ImagenID",
                table: "Servicios",
                column: "ImagenID");

            migrationBuilder.AddForeignKey(
                name: "FK_Servicios_Imagen_ImagenID",
                table: "Servicios",
                column: "ImagenID",
                principalTable: "Imagen",
                principalColumn: "ImagenID");
        }
    }
}

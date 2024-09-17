using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Postulate.Migrations
{
    /// <inheritdoc />
    public partial class Migracion_ImgTrabajo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Trabajos_Imagen_ImagenID",
                table: "Trabajos");

            migrationBuilder.AlterColumn<int>(
                name: "ImagenID",
                table: "Trabajos",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Trabajos_Imagen_ImagenID",
                table: "Trabajos",
                column: "ImagenID",
                principalTable: "Imagen",
                principalColumn: "ImagenID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Trabajos_Imagen_ImagenID",
                table: "Trabajos");

            migrationBuilder.AlterColumn<int>(
                name: "ImagenID",
                table: "Trabajos",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Trabajos_Imagen_ImagenID",
                table: "Trabajos",
                column: "ImagenID",
                principalTable: "Imagen",
                principalColumn: "ImagenID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

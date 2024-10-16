using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Postulate.Migrations
{
    /// <inheritdoc />
    public partial class AgregamosEnumContrato : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "RespuestaDesolicitud",
                table: "ContratoRespondidos",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RespuestaDesolicitud",
                table: "ContratoRespondidos");
        }
    }
}

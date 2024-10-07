using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Postulate.Migrations
{
    /// <inheritdoc />
    public partial class AgregamosTrabajoEnservicios : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "FechaMatch",
                table: "ContratoRespondidos",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "ProfesionID",
                table: "ContratoRespondidos",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FechaMatch",
                table: "ContratoRespondidos");

            migrationBuilder.DropColumn(
                name: "ProfesionID",
                table: "ContratoRespondidos");
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ControlGastos.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddCamposGastoFijoEnGasto : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "EsGastoFijo",
                table: "Gastos",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "GastoFijoId",
                table: "Gastos",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Gastos_GastoFijoId",
                table: "Gastos",
                column: "GastoFijoId");

            migrationBuilder.AddForeignKey(
                name: "FK_Gastos_GastosFijos_GastoFijoId",
                table: "Gastos",
                column: "GastoFijoId",
                principalTable: "GastosFijos",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Gastos_GastosFijos_GastoFijoId",
                table: "Gastos");

            migrationBuilder.DropIndex(
                name: "IX_Gastos_GastoFijoId",
                table: "Gastos");

            migrationBuilder.DropColumn(
                name: "EsGastoFijo",
                table: "Gastos");

            migrationBuilder.DropColumn(
                name: "GastoFijoId",
                table: "Gastos");
        }
    }
}

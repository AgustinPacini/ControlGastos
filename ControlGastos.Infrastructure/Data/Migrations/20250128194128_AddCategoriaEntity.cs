using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ControlGastos.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddCategoriaEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CategoriaId",
                table: "Ingresos",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "CategoriaId",
                table: "Gastos",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Categorias",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categorias", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Ingresos_CategoriaId",
                table: "Ingresos",
                column: "CategoriaId");

            migrationBuilder.CreateIndex(
                name: "IX_Gastos_CategoriaId",
                table: "Gastos",
                column: "CategoriaId");

            migrationBuilder.AddForeignKey(
                name: "FK_Gastos_Categorias_CategoriaId",
                table: "Gastos",
                column: "CategoriaId",
                principalTable: "Categorias",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Ingresos_Categorias_CategoriaId",
                table: "Ingresos",
                column: "CategoriaId",
                principalTable: "Categorias",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Gastos_Categorias_CategoriaId",
                table: "Gastos");

            migrationBuilder.DropForeignKey(
                name: "FK_Ingresos_Categorias_CategoriaId",
                table: "Ingresos");

            migrationBuilder.DropTable(
                name: "Categorias");

            migrationBuilder.DropIndex(
                name: "IX_Ingresos_CategoriaId",
                table: "Ingresos");

            migrationBuilder.DropIndex(
                name: "IX_Gastos_CategoriaId",
                table: "Gastos");

            migrationBuilder.DropColumn(
                name: "CategoriaId",
                table: "Ingresos");

            migrationBuilder.DropColumn(
                name: "CategoriaId",
                table: "Gastos");
        }
    }
}

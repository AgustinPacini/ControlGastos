using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ControlGastos.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddTipoCategoriaToCategorias : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Notas",
                table: "Ingresos",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<decimal>(
                name: "Monto",
                table: "Ingresos",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<string>(
                name: "TipoCategoria",
                table: "Categorias",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "Ambos");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TipoCategoria",
                table: "Categorias");

            migrationBuilder.AlterColumn<string>(
                name: "Notas",
                table: "Ingresos",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Monto",
                table: "Ingresos",
                type: "int",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");
        }
    }
}

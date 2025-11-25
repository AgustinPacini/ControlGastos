using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ControlGastos.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class MultiusuarioCuentasGastosFijos : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UsuarioId",
                table: "Presupuestos",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "CuentaId",
                table: "Ingresos",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UsuarioId",
                table: "Ingresos",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "CuentaId",
                table: "Gastos",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UsuarioId",
                table: "Gastos",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Cuentas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Tipo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SaldoInicial = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    SaldoActual = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    UsuarioId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cuentas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Cuentas_Usuarios_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "MetaAhorros",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NombreObjetivo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MontoObjetivo = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    MontoAhorrado = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    FechaObjetivo = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FechaCreacion = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UsuarioId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MetaAhorros", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MetaAhorros_Usuarios_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "GastosFijos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Descripcion = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Monto = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    FechaInicio = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Periodicidad = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DiaReferencia = table.Column<int>(type: "int", nullable: false),
                    Activo = table.Column<bool>(type: "bit", nullable: false),
                    UsuarioId = table.Column<int>(type: "int", nullable: false),
                    CategoriaId = table.Column<int>(type: "int", nullable: true),
                    CuentaId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GastosFijos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GastosFijos_Categorias_CategoriaId",
                        column: x => x.CategoriaId,
                        principalTable: "Categorias",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_GastosFijos_Cuentas_CuentaId",
                        column: x => x.CuentaId,
                        principalTable: "Cuentas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_GastosFijos_Usuarios_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Presupuestos_UsuarioId",
                table: "Presupuestos",
                column: "UsuarioId");

            migrationBuilder.CreateIndex(
                name: "IX_Ingresos_CuentaId",
                table: "Ingresos",
                column: "CuentaId");

            migrationBuilder.CreateIndex(
                name: "IX_Ingresos_UsuarioId",
                table: "Ingresos",
                column: "UsuarioId");

            migrationBuilder.CreateIndex(
                name: "IX_Gastos_CuentaId",
                table: "Gastos",
                column: "CuentaId");

            migrationBuilder.CreateIndex(
                name: "IX_Gastos_UsuarioId",
                table: "Gastos",
                column: "UsuarioId");

            migrationBuilder.CreateIndex(
                name: "IX_Cuentas_UsuarioId",
                table: "Cuentas",
                column: "UsuarioId");

            migrationBuilder.CreateIndex(
                name: "IX_GastosFijos_CategoriaId",
                table: "GastosFijos",
                column: "CategoriaId");

            migrationBuilder.CreateIndex(
                name: "IX_GastosFijos_CuentaId",
                table: "GastosFijos",
                column: "CuentaId");

            migrationBuilder.CreateIndex(
                name: "IX_GastosFijos_UsuarioId",
                table: "GastosFijos",
                column: "UsuarioId");

            migrationBuilder.CreateIndex(
                name: "IX_MetaAhorros_UsuarioId",
                table: "MetaAhorros",
                column: "UsuarioId");
            migrationBuilder.Sql(@"
                 -- Si hay al menos un usuario
                 IF EXISTS (SELECT 1 FROM Usuarios)
                 BEGIN
                     DECLARE @UserId INT;
                     -- Tomamos el primer usuario de la tabla
                     SELECT TOP(1) @UserId = Id FROM Usuarios ORDER BY Id ASC;

                     -- Asignamos ese usuario a los registros que quedaron con 0
                     UPDATE Gastos SET UsuarioId = @UserId WHERE UsuarioId = 0;
                     UPDATE Ingresos SET UsuarioId = @UserId WHERE UsuarioId = 0;
                     UPDATE Presupuestos SET UsuarioId = @UserId WHERE UsuarioId = 0;
                 END
            ");
            migrationBuilder.AddForeignKey(
                name: "FK_Gastos_Cuentas_CuentaId",
                table: "Gastos",
                column: "CuentaId",
                principalTable: "Cuentas",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Gastos_Usuarios_UsuarioId",
                table: "Gastos",
                column: "UsuarioId",
                principalTable: "Usuarios",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Ingresos_Cuentas_CuentaId",
                table: "Ingresos",
                column: "CuentaId",
                principalTable: "Cuentas",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Ingresos_Usuarios_UsuarioId",
                table: "Ingresos",
                column: "UsuarioId",
                principalTable: "Usuarios",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Presupuestos_Usuarios_UsuarioId",
                table: "Presupuestos",
                column: "UsuarioId",
                principalTable: "Usuarios",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Gastos_Cuentas_CuentaId",
                table: "Gastos");

            migrationBuilder.DropForeignKey(
                name: "FK_Gastos_Usuarios_UsuarioId",
                table: "Gastos");

            migrationBuilder.DropForeignKey(
                name: "FK_Ingresos_Cuentas_CuentaId",
                table: "Ingresos");

            migrationBuilder.DropForeignKey(
                name: "FK_Ingresos_Usuarios_UsuarioId",
                table: "Ingresos");

            migrationBuilder.DropForeignKey(
                name: "FK_Presupuestos_Usuarios_UsuarioId",
                table: "Presupuestos");

            migrationBuilder.DropTable(
                name: "GastosFijos");

            migrationBuilder.DropTable(
                name: "MetaAhorros");

            migrationBuilder.DropTable(
                name: "Cuentas");

            migrationBuilder.DropIndex(
                name: "IX_Presupuestos_UsuarioId",
                table: "Presupuestos");

            migrationBuilder.DropIndex(
                name: "IX_Ingresos_CuentaId",
                table: "Ingresos");

            migrationBuilder.DropIndex(
                name: "IX_Ingresos_UsuarioId",
                table: "Ingresos");

            migrationBuilder.DropIndex(
                name: "IX_Gastos_CuentaId",
                table: "Gastos");

            migrationBuilder.DropIndex(
                name: "IX_Gastos_UsuarioId",
                table: "Gastos");

            migrationBuilder.DropColumn(
                name: "UsuarioId",
                table: "Presupuestos");

            migrationBuilder.DropColumn(
                name: "CuentaId",
                table: "Ingresos");

            migrationBuilder.DropColumn(
                name: "UsuarioId",
                table: "Ingresos");

            migrationBuilder.DropColumn(
                name: "CuentaId",
                table: "Gastos");

            migrationBuilder.DropColumn(
                name: "UsuarioId",
                table: "Gastos");
        }
    }
}

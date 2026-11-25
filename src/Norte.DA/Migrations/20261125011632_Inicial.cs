using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Norte.DA.Migrations
{
    /// <inheritdoc />
    public partial class Inicial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "categorias",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Nombre = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: false),
                    Tipo = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_categorias", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "usuarios",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Nombre = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Correo = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    HashContrasena = table.Column<string>(type: "text", nullable: false),
                    FechaRegistroUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_usuarios", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "deudas",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UsuarioId = table.Column<Guid>(type: "uuid", nullable: false),
                    Nombre = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Tipo = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    MontoTotal = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    MontoPendiente = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    FechaVencimiento = table.Column<DateOnly>(type: "date", nullable: false),
                    EsRecurrente = table.Column<bool>(type: "boolean", nullable: false),
                    xmin = table.Column<uint>(type: "xid", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_deudas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_deudas_usuarios_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "metas_ahorro",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UsuarioId = table.Column<Guid>(type: "uuid", nullable: false),
                    Nombre = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    MontoObjetivo = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    FechaLimite = table.Column<DateOnly>(type: "date", nullable: false),
                    xmin = table.Column<uint>(type: "xid", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_metas_ahorro", x => x.Id);
                    table.ForeignKey(
                        name: "FK_metas_ahorro_usuarios_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "presupuestos",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UsuarioId = table.Column<Guid>(type: "uuid", nullable: false),
                    CategoriaId = table.Column<Guid>(type: "uuid", nullable: false),
                    Anio = table.Column<int>(type: "integer", nullable: false),
                    Mes = table.Column<int>(type: "integer", nullable: false),
                    MontoLimite = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    xmin = table.Column<uint>(type: "xid", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_presupuestos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_presupuestos_categorias_CategoriaId",
                        column: x => x.CategoriaId,
                        principalTable: "categorias",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_presupuestos_usuarios_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "transacciones",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UsuarioId = table.Column<Guid>(type: "uuid", nullable: false),
                    CategoriaId = table.Column<Guid>(type: "uuid", nullable: false),
                    Tipo = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    Monto = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    Fecha = table.Column<DateOnly>(type: "date", nullable: false),
                    Descripcion = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: true),
                    FechaRegistroUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_transacciones", x => x.Id);
                    table.ForeignKey(
                        name: "FK_transacciones_categorias_CategoriaId",
                        column: x => x.CategoriaId,
                        principalTable: "categorias",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_transacciones_usuarios_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "pagos_deuda",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    DeudaId = table.Column<Guid>(type: "uuid", nullable: false),
                    Monto = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    Fecha = table.Column<DateOnly>(type: "date", nullable: false),
                    EsAutomatico = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_pagos_deuda", x => x.Id);
                    table.ForeignKey(
                        name: "FK_pagos_deuda_deudas_DeudaId",
                        column: x => x.DeudaId,
                        principalTable: "deudas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "aportes_meta",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    MetaAhorroId = table.Column<Guid>(type: "uuid", nullable: false),
                    Monto = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    Fecha = table.Column<DateOnly>(type: "date", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_aportes_meta", x => x.Id);
                    table.ForeignKey(
                        name: "FK_aportes_meta_metas_ahorro_MetaAhorroId",
                        column: x => x.MetaAhorroId,
                        principalTable: "metas_ahorro",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "categorias",
                columns: new[] { "Id", "Nombre", "Tipo" },
                values: new object[,]
                {
                    { new Guid("8f0f6f3a-0001-4c1a-9a01-000000000001"), "Salario", "Ingreso" },
                    { new Guid("8f0f6f3a-0001-4c1a-9a01-000000000002"), "Trabajo independiente", "Ingreso" },
                    { new Guid("8f0f6f3a-0001-4c1a-9a01-000000000003"), "Otros ingresos", "Ingreso" },
                    { new Guid("8f0f6f3a-0001-4c1a-9a01-000000000004"), "Alimentación", "Gasto" },
                    { new Guid("8f0f6f3a-0001-4c1a-9a01-000000000005"), "Transporte", "Gasto" },
                    { new Guid("8f0f6f3a-0001-4c1a-9a01-000000000006"), "Servicios", "Gasto" },
                    { new Guid("8f0f6f3a-0001-4c1a-9a01-000000000007"), "Entretenimiento", "Gasto" },
                    { new Guid("8f0f6f3a-0001-4c1a-9a01-000000000008"), "Suscripciones", "Gasto" },
                    { new Guid("8f0f6f3a-0001-4c1a-9a01-000000000009"), "Salud", "Gasto" },
                    { new Guid("8f0f6f3a-0001-4c1a-9a01-00000000000a"), "Educación", "Gasto" },
                    { new Guid("8f0f6f3a-0001-4c1a-9a01-00000000000b"), "Otros gastos", "Gasto" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_aportes_meta_MetaAhorroId",
                table: "aportes_meta",
                column: "MetaAhorroId");

            migrationBuilder.CreateIndex(
                name: "IX_deudas_UsuarioId",
                table: "deudas",
                column: "UsuarioId");

            migrationBuilder.CreateIndex(
                name: "IX_metas_ahorro_UsuarioId",
                table: "metas_ahorro",
                column: "UsuarioId");

            migrationBuilder.CreateIndex(
                name: "IX_pagos_deuda_DeudaId",
                table: "pagos_deuda",
                column: "DeudaId");

            migrationBuilder.CreateIndex(
                name: "IX_presupuestos_CategoriaId",
                table: "presupuestos",
                column: "CategoriaId");

            migrationBuilder.CreateIndex(
                name: "IX_presupuestos_UsuarioId_CategoriaId_Anio_Mes",
                table: "presupuestos",
                columns: new[] { "UsuarioId", "CategoriaId", "Anio", "Mes" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_transacciones_CategoriaId",
                table: "transacciones",
                column: "CategoriaId");

            migrationBuilder.CreateIndex(
                name: "IX_transacciones_UsuarioId_Fecha",
                table: "transacciones",
                columns: new[] { "UsuarioId", "Fecha" });

            migrationBuilder.CreateIndex(
                name: "IX_usuarios_Correo",
                table: "usuarios",
                column: "Correo",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "aportes_meta");

            migrationBuilder.DropTable(
                name: "pagos_deuda");

            migrationBuilder.DropTable(
                name: "presupuestos");

            migrationBuilder.DropTable(
                name: "transacciones");

            migrationBuilder.DropTable(
                name: "metas_ahorro");

            migrationBuilder.DropTable(
                name: "deudas");

            migrationBuilder.DropTable(
                name: "categorias");

            migrationBuilder.DropTable(
                name: "usuarios");
        }
    }
}

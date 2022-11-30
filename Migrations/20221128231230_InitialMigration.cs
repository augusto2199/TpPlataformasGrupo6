using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InterfazTP.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Caja_de_Ahorro",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    cbu = table.Column<int>(type: "int", nullable: false),
                    saldo = table.Column<float>(type: "real", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Caja_de_Ahorro", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Usuarios",
                columns: table => new
                {
                    idusuario = table.Column<int>(name: "id_usuario", type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    dni = table.Column<int>(type: "int", nullable: false),
                    nombre = table.Column<string>(type: "varchar(50)", nullable: false),
                    apellido = table.Column<string>(type: "varchar(50)", nullable: false),
                    email = table.Column<string>(type: "varchar(512)", nullable: false),
                    usuario = table.Column<string>(type: "varchar(50)", nullable: false),
                    password = table.Column<string>(type: "varchar(50)", nullable: false),
                    intentosFallidos = table.Column<int>(type: "int", nullable: false),
                    bloqueado = table.Column<bool>(type: "bit", nullable: false),
                    administrador = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Usuarios", x => x.idusuario);
                });

            migrationBuilder.CreateTable(
                name: "Movimientos",
                columns: table => new
                {
                    idmovimiento = table.Column<int>(name: "id_movimiento", type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    numcaja = table.Column<int>(name: "num_caja", type: "int", nullable: false),
                    detalle = table.Column<string>(type: "varchar(50)", nullable: false),
                    monto = table.Column<float>(type: "real", nullable: false),
                    fecha = table.Column<DateTime>(type: "date", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Movimientos", x => x.idmovimiento);
                    table.ForeignKey(
                        name: "FK_Movimientos_Caja_de_Ahorro_num_caja",
                        column: x => x.numcaja,
                        principalTable: "Caja_de_Ahorro",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Pagos",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    numusr = table.Column<int>(name: "num_usr", type: "int", nullable: false),
                    nombre = table.Column<string>(type: "varchar(50)", nullable: false),
                    monto = table.Column<float>(type: "real", nullable: false),
                    pagado = table.Column<bool>(type: "bit", nullable: false),
                    metodo = table.Column<string>(type: "varchar(50)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pagos", x => x.id);
                    table.ForeignKey(
                        name: "FK_Pagos_Usuarios_num_usr",
                        column: x => x.numusr,
                        principalTable: "Usuarios",
                        principalColumn: "id_usuario",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Plazo_Fijo",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    numusr = table.Column<int>(name: "num_usr", type: "int", nullable: false),
                    monto = table.Column<float>(type: "real", nullable: false),
                    fechaIni = table.Column<DateTime>(type: "date", nullable: false),
                    fechaFin = table.Column<DateTime>(type: "date", nullable: false),
                    tasa = table.Column<float>(type: "real", nullable: false),
                    pagado = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Plazo_Fijo", x => x.id);
                    table.ForeignKey(
                        name: "FK_Plazo_Fijo_Usuarios_num_usr",
                        column: x => x.numusr,
                        principalTable: "Usuarios",
                        principalColumn: "id_usuario",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Tarjeta_de_Credito",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    numusr = table.Column<int>(name: "num_usr", type: "int", nullable: false),
                    numero = table.Column<int>(type: "int", nullable: false),
                    codigoV = table.Column<int>(type: "int", nullable: false),
                    limite = table.Column<float>(type: "real", nullable: false),
                    consumos = table.Column<float>(type: "real", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tarjeta_de_Credito", x => x.id);
                    table.ForeignKey(
                        name: "FK_Tarjeta_de_Credito_Usuarios_num_usr",
                        column: x => x.numusr,
                        principalTable: "Usuarios",
                        principalColumn: "id_usuario",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UsuarioCajaDeAhorro",
                columns: table => new
                {
                    idUsuarioCaja = table.Column<int>(name: "id_UsuarioCaja", type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    fkusuario = table.Column<int>(name: "fk_usuario", type: "int", nullable: false),
                    fkcajaAhorro = table.Column<int>(name: "fk_cajaAhorro", type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UsuarioCajaDeAhorro", x => x.idUsuarioCaja);
                    table.ForeignKey(
                        name: "FK_UsuarioCajaDeAhorro_Caja_de_Ahorro_fk_cajaAhorro",
                        column: x => x.fkcajaAhorro,
                        principalTable: "Caja_de_Ahorro",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UsuarioCajaDeAhorro_Usuarios_fk_usuario",
                        column: x => x.fkusuario,
                        principalTable: "Usuarios",
                        principalColumn: "id_usuario",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Movimientos_num_caja",
                table: "Movimientos",
                column: "num_caja");

            migrationBuilder.CreateIndex(
                name: "IX_Pagos_num_usr",
                table: "Pagos",
                column: "num_usr");

            migrationBuilder.CreateIndex(
                name: "IX_Plazo_Fijo_num_usr",
                table: "Plazo_Fijo",
                column: "num_usr");

            migrationBuilder.CreateIndex(
                name: "IX_Tarjeta_de_Credito_num_usr",
                table: "Tarjeta_de_Credito",
                column: "num_usr");

            migrationBuilder.CreateIndex(
                name: "IX_UsuarioCajaDeAhorro_fk_cajaAhorro",
                table: "UsuarioCajaDeAhorro",
                column: "fk_cajaAhorro");

            migrationBuilder.CreateIndex(
                name: "IX_UsuarioCajaDeAhorro_fk_usuario",
                table: "UsuarioCajaDeAhorro",
                column: "fk_usuario");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Movimientos");

            migrationBuilder.DropTable(
                name: "Pagos");

            migrationBuilder.DropTable(
                name: "Plazo_Fijo");

            migrationBuilder.DropTable(
                name: "Tarjeta_de_Credito");

            migrationBuilder.DropTable(
                name: "UsuarioCajaDeAhorro");

            migrationBuilder.DropTable(
                name: "Caja_de_Ahorro");

            migrationBuilder.DropTable(
                name: "Usuarios");
        }
    }
}

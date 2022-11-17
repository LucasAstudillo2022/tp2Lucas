using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace prueba.Data.Migrations
{
    public partial class init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Areatrabajo",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    area = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Areatrabajo", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Mutual",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    mutualpers = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Mutual", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Datopersona",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nombre = table.Column<string>(nullable: true),
                    apellido = table.Column<string>(nullable: true),
                    fotopersona = table.Column<int>(nullable: false),
                    areaid = table.Column<int>(nullable: false),
                    AreatrabajoId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Datopersona", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Datopersona_Areatrabajo_AreatrabajoId",
                        column: x => x.AreatrabajoId,
                        principalTable: "Areatrabajo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Cooperativa",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    nombre = table.Column<string>(nullable: true),
                    biografia = table.Column<string>(nullable: true),
                    fechacreacion = table.Column<DateTime>(nullable: false),
                    fotocoop = table.Column<int>(nullable: false),
                    mutualid = table.Column<int>(nullable: false),
                    personaid = table.Column<int>(nullable: false),
                    datopersonalesId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cooperativa", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Cooperativa_Datopersona_datopersonalesId",
                        column: x => x.datopersonalesId,
                        principalTable: "Datopersona",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Cooperativa_Mutual_mutualid",
                        column: x => x.mutualid,
                        principalTable: "Mutual",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Cooperativa_datopersonalesId",
                table: "Cooperativa",
                column: "datopersonalesId");

            migrationBuilder.CreateIndex(
                name: "IX_Cooperativa_mutualid",
                table: "Cooperativa",
                column: "mutualid");

            migrationBuilder.CreateIndex(
                name: "IX_Datopersona_AreatrabajoId",
                table: "Datopersona",
                column: "AreatrabajoId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Cooperativa");

            migrationBuilder.DropTable(
                name: "Datopersona");

            migrationBuilder.DropTable(
                name: "Mutual");

            migrationBuilder.DropTable(
                name: "Areatrabajo");
        }
    }
}

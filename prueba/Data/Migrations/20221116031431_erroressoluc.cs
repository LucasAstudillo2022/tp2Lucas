using Microsoft.EntityFrameworkCore.Migrations;

namespace prueba.Data.Migrations
{
    public partial class erroressoluc : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cooperativa_Datopersona_datopersonalesId",
                table: "Cooperativa");

            migrationBuilder.DropIndex(
                name: "IX_Cooperativa_datopersonalesId",
                table: "Cooperativa");

            migrationBuilder.DropColumn(
                name: "datopersonalesId",
                table: "Cooperativa");

            migrationBuilder.RenameColumn(
                name: "nombre",
                table: "Cooperativa",
                newName: "Nombre");

            migrationBuilder.AddColumn<int>(
                name: "nombreId",
                table: "Cooperativa",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Cooperativa_nombreId",
                table: "Cooperativa",
                column: "nombreId");

            migrationBuilder.AddForeignKey(
                name: "FK_Cooperativa_Datopersona_nombreId",
                table: "Cooperativa",
                column: "nombreId",
                principalTable: "Datopersona",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cooperativa_Datopersona_nombreId",
                table: "Cooperativa");

            migrationBuilder.DropIndex(
                name: "IX_Cooperativa_nombreId",
                table: "Cooperativa");

            migrationBuilder.DropColumn(
                name: "nombreId",
                table: "Cooperativa");

            migrationBuilder.RenameColumn(
                name: "Nombre",
                table: "Cooperativa",
                newName: "nombre");

            migrationBuilder.AddColumn<int>(
                name: "datopersonalesId",
                table: "Cooperativa",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Cooperativa_datopersonalesId",
                table: "Cooperativa",
                column: "datopersonalesId");

            migrationBuilder.AddForeignKey(
                name: "FK_Cooperativa_Datopersona_datopersonalesId",
                table: "Cooperativa",
                column: "datopersonalesId",
                principalTable: "Datopersona",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}

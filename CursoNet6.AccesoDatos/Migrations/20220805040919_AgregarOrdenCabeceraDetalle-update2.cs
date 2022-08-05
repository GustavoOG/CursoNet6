using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CursoNet6.AccesoDatos.Migrations
{
    public partial class AgregarOrdenCabeceraDetalleupdate2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orden_AspNetUsers_UsuararioAplicacionId",
                table: "Orden");

            migrationBuilder.DropIndex(
                name: "IX_Orden_UsuararioAplicacionId",
                table: "Orden");

            migrationBuilder.DropColumn(
                name: "UsuararioAplicacionId",
                table: "Orden");

            migrationBuilder.AlterColumn<string>(
                name: "UsuarioAplicacionId",
                table: "Orden",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_Orden_UsuarioAplicacionId",
                table: "Orden",
                column: "UsuarioAplicacionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Orden_AspNetUsers_UsuarioAplicacionId",
                table: "Orden",
                column: "UsuarioAplicacionId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orden_AspNetUsers_UsuarioAplicacionId",
                table: "Orden");

            migrationBuilder.DropIndex(
                name: "IX_Orden_UsuarioAplicacionId",
                table: "Orden");

            migrationBuilder.AlterColumn<string>(
                name: "UsuarioAplicacionId",
                table: "Orden",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<string>(
                name: "UsuararioAplicacionId",
                table: "Orden",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Orden_UsuararioAplicacionId",
                table: "Orden",
                column: "UsuararioAplicacionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Orden_AspNetUsers_UsuararioAplicacionId",
                table: "Orden",
                column: "UsuararioAplicacionId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}

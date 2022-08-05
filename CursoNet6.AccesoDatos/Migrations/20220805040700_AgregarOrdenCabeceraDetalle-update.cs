using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CursoNet6.AccesoDatos.Migrations
{
    public partial class AgregarOrdenCabeceraDetalleupdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UsuarioAplacacionId",
                table: "Orden",
                newName: "UsuarioAplicacionId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UsuarioAplicacionId",
                table: "Orden",
                newName: "UsuarioAplacacionId");
        }
    }
}

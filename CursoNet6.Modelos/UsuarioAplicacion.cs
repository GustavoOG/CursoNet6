using Microsoft.AspNetCore.Identity;

namespace CursoNet6.Modelos
{
    public class UsuarioAplicacion : IdentityUser
    {
        public string NombreCompleto { get; set; }
    }
}

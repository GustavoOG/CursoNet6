using Microsoft.AspNetCore.Identity;

namespace CursoNet6.Models
{
    public class UsuarioAplicacion : IdentityUser
    {
        public string NombreCompleto { get; set; }
    }
}

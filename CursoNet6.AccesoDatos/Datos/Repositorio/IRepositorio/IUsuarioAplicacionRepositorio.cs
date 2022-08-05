using CursoNet6.Modelos;

namespace CursoNet6.AccesoDatos.Datos.Repositorio.IRepositorio
{
    public interface IUsuarioAplicacionRepositorio : IRepositorio<UsuarioAplicacion>
    {

        void Actualizar(UsuarioAplicacion usuarioAplicacion);

    }
}

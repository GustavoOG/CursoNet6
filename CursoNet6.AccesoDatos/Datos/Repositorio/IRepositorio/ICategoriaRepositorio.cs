using CursoNet6.Modelos;

namespace CursoNet6.AccesoDatos.Datos.Repositorio.IRepositorio
{
    public interface ICategoriaRepositorio : IRepositorio<Categoria>
    {

        void Actualizar(Categoria categoria);
    }
}

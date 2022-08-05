using CursoNet6.Modelos;

namespace CursoNet6.AccesoDatos.Datos.Repositorio.IRepositorio
{
    public interface IOrdenRepositorio : IRepositorio<Orden>
    {

        void Actualizar(Orden orden);

    }
}

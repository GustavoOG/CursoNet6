using CursoNet6.Modelos;

namespace CursoNet6.AccesoDatos.Datos.Repositorio.IRepositorio
{
    public interface ITipoAplicacionRepositorio : IRepositorio<TipoAplicacion>
    {

        void Actualizar(TipoAplicacion tipoAplicacion);
    }
}

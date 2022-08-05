using CursoNet6.Modelos;

namespace CursoNet6.AccesoDatos.Datos.Repositorio.IRepositorio
{
    public interface IOrdenDetalleRepositorio : IRepositorio<OrdenDetalle>
    {

        void Actualizar(OrdenDetalle ordenDetalle);

    }
}

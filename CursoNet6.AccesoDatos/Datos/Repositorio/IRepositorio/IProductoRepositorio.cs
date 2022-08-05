using CursoNet6.Modelos;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CursoNet6.AccesoDatos.Datos.Repositorio.IRepositorio
{
    public interface IProductoRepositorio : IRepositorio<Producto>
    {

        void Actualizar(Producto producto);

        IEnumerable<SelectListItem> ObtenerTodosDownList(string obj);
    }
}

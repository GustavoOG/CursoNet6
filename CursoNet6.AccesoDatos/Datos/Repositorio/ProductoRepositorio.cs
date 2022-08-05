using CursoNet6.AccesoDatos.Datos.Repositorio.IRepositorio;
using CursoNet6.Modelos;
using CursoNet6.Utilidades;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CursoNet6.AccesoDatos.Datos.Repositorio
{
    public class ProductoRepositorio : Repositorio<Producto>, IProductoRepositorio
    {
        private readonly ApplicationDbContext _db;

        public ProductoRepositorio(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Actualizar(Producto producto)
        {
            _db.Update(producto);
        }

        public IEnumerable<SelectListItem> ObtenerTodosDownList(string obj)
        {
            if (obj == WC.CategoriaNombre)
            {
                return _db.Categoria.Select(m => new SelectListItem()
                {
                    Text = m.NombreCategoria,
                    Value = m.Id.ToString()
                });
            }
            else if (obj == WC.TipoAplicacionNombre)
            {
                return _db.TipoAplicacion.Select(m => new SelectListItem()
                {
                    Text = m.Nombre,
                    Value = m.Id.ToString()
                });
            }
            else
            {
                return null;
            }
        }
    }
}

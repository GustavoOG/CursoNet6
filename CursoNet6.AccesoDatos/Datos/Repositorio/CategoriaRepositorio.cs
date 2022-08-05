using CursoNet6.AccesoDatos.Datos.Repositorio.IRepositorio;
using CursoNet6.Modelos;

namespace CursoNet6.AccesoDatos.Datos.Repositorio
{
    public class CategoriaRepositorio : Repositorio<Categoria>, ICategoriaRepositorio
    {
        private readonly ApplicationDbContext _db;

        public CategoriaRepositorio(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Actualizar(Categoria categoria)
        {
            var catAnterior = _db.Categoria.FirstOrDefault(c => c.Id.Equals(categoria.Id));
            if (catAnterior != null)
            {
                catAnterior.NombreCategoria = categoria.NombreCategoria;
                categoria.MostrarOrden = categoria.MostrarOrden;
                _db.SaveChanges();
            }
        }
    }
}

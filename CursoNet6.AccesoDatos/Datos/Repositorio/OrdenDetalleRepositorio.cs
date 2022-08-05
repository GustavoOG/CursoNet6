using CursoNet6.AccesoDatos.Datos.Repositorio.IRepositorio;
using CursoNet6.Modelos;

namespace CursoNet6.AccesoDatos.Datos.Repositorio
{
    public class OrdenDetalleRepositorio : Repositorio<OrdenDetalle>, IOrdenDetalleRepositorio
    {
        private readonly ApplicationDbContext _db;

        public OrdenDetalleRepositorio(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Actualizar(OrdenDetalle ordenDetalle)
        {
            _db.Update(ordenDetalle);
        }


    }
}

using CursoNet6.AccesoDatos.Datos.Repositorio.IRepositorio;
using CursoNet6.Modelos;

namespace CursoNet6.AccesoDatos.Datos.Repositorio
{
    public class TipoAplicacionRepositorio : Repositorio<TipoAplicacion>, ITipoAplicacionRepositorio
    {
        private readonly ApplicationDbContext _db;

        public TipoAplicacionRepositorio(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Actualizar(TipoAplicacion tipoAplicacion)
        {
            var catTipoAPlicacion = _db.TipoAplicacion.FirstOrDefault(c => c.Id.Equals(tipoAplicacion.Id));
            if (catTipoAPlicacion != null)
            {
                catTipoAPlicacion.Nombre = tipoAplicacion.Nombre;
                _db.SaveChanges();
            }
        }
    }
}

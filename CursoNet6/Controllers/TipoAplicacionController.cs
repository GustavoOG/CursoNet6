
using CursoNet6.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CursoNet6.Controllers
{

    [Authorize(Roles = WC.AdminRole)]
    public class TipoaplicacionController : Controller
    {
        private readonly ApplicationDbContext _db;
        public TipoaplicacionController(ApplicationDbContext db)
        {
            this._db = db;
        }

        public IActionResult Index()
        {
            IEnumerable<TipoAplicacion> lista = _db.TipoAplicacion;

            return View(lista);
        }

        //Get
        public IActionResult Crear()
        {

            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Crear(TipoAplicacion tipoAplicacion)
        {
            if (ModelState.IsValid)
            {
                _db.TipoAplicacion.Add(tipoAplicacion);
                _db.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(tipoAplicacion);
        }

        public IActionResult Editar(int? Id)
        {
            if (!Id.HasValue)
            {
                return NotFound();
            }
            var obj = _db.TipoAplicacion.Find(Id);
            if (obj == null)
            {
                return NotFound();
            }
            return View(obj);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Editar(TipoAplicacion tipoAplicacion)
        {
            if (ModelState.IsValid)
            {
                _db.TipoAplicacion.Update(tipoAplicacion);
                _db.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(tipoAplicacion);
        }


        public IActionResult Eliminar(int? Id)
        {
            if (!Id.HasValue)
            {
                return NotFound();
            }
            var obj = _db.TipoAplicacion.Find(Id);
            if (obj == null)
            {
                return NotFound();
            }
            return View(obj);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Eliminar(TipoAplicacion tipoAplicacion)
        {
            if (tipoAplicacion == null)
            {
                return NotFound();
            }
            _db.TipoAplicacion.Remove(tipoAplicacion);
            _db.SaveChanges();
            return RedirectToAction(nameof(Index));

            return View(tipoAplicacion);
        }
    }
}

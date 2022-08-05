
using CursoNet6.Modelos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CursoNet6.Controllers
{

    [Authorize(Roles = WC.AdminRole)]
    public class TipoaplicacionController : Controller
    {
        private readonly ITipoAplicacionRepositorio _tipoRepo;
        public TipoaplicacionController(ITipoAplicacionRepositorio tipoRepo)
        {
            this._tipoRepo = tipoRepo;
        }

        public IActionResult Index()
        {
            IEnumerable<TipoAplicacion> lista = _tipoRepo.ObtenerTodos();

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
                _tipoRepo.Agregar(tipoAplicacion);
                _tipoRepo.Grabar();
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
            var obj = _tipoRepo.Obtener(Id.GetValueOrDefault());
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
                _tipoRepo.Actualizar(tipoAplicacion);
                _tipoRepo.Grabar();
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
            var obj = _tipoRepo.Obtener(Id.GetValueOrDefault());
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
            _tipoRepo.Remover(tipoAplicacion);
            _tipoRepo.Grabar();
            return RedirectToAction(nameof(Index));
        }
    }
}

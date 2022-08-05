using CursoNet6.Modelos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CursoNet6.Controllers
{
    [Authorize(Roles = WC.AdminRole)]
    public class CategoriaController : Controller
    {
        private readonly ICategoriaRepositorio _catRepo;
        public CategoriaController(ICategoriaRepositorio catRepo)
        {
            this._catRepo = catRepo;
        }

        public IActionResult Index()
        {
            IEnumerable<Categoria> lista = _catRepo.ObtenerTodos();

            return View(lista);
        }

        //Get
        public IActionResult Crear()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Crear(Categoria categoria)
        {
            if (ModelState.IsValid)
            {
                _catRepo.Agregar(categoria);
                _catRepo.Grabar();
                return RedirectToAction(nameof(Index));
            }
            return View(categoria);
        }

        public IActionResult Editar(int? Id)
        {
            if (!Id.HasValue)
            {
                return NotFound();
            }
            var obj = _catRepo.Obtener(Id.GetValueOrDefault());
            if (obj == null)
            {
                return NotFound();
            }
            return View(obj);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Editar(Categoria categoria)
        {
            if (ModelState.IsValid)
            {
                _catRepo.Actualizar(categoria);
                _catRepo.Grabar();
                return RedirectToAction(nameof(Index));
            }
            return View(categoria);
        }


        public IActionResult Eliminar(int? Id)
        {
            if (!Id.HasValue)
            {
                return NotFound();
            }
            var obj = _catRepo.Obtener(Id.GetValueOrDefault());
            if (obj == null)
            {
                return NotFound();
            }
            return View(obj);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Eliminar(Categoria categoria)
        {
            if (categoria == null)
            {
                return NotFound();
            }
            _catRepo.Remover(categoria);
            _catRepo.Grabar();
            return RedirectToAction(nameof(Index));

        }
    }
}

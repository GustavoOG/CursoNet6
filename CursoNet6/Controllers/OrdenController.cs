using CursoNet6.Modelos.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace CursoNet6.Controllers
{
    public class OrdenController : Controller
    {
        private readonly IOrdenRepositorio _ordenRepositorio;
        private readonly IOrdenDetalleRepositorio _ordenDetalleRepositorio;

        [BindProperty]
        private OrdenVM ordenVM { get; set; }
        public OrdenController(IOrdenRepositorio ordenRepositorio, IOrdenDetalleRepositorio ordenDetalleRepositorio)
        {
            _ordenRepositorio = ordenRepositorio;
            _ordenDetalleRepositorio = ordenDetalleRepositorio;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Detalle(int id)
        {

            ordenVM = new OrdenVM()
            {
                Orden = _ordenRepositorio.ObtenerPrimero(m => m.Id == id),
                OrdenDetalle = _ordenDetalleRepositorio.ObtenerTodos(m => m.OrdenId == id, incluirPropiedades: "Producto")
            };

            return View(ordenVM);

        }

        #region APIs

        [HttpGet]
        public IActionResult ObtenerListaOrdenes()
        {
            return Json(new { data = _ordenRepositorio.ObtenerTodos() });
        }

        #endregion
    }
}

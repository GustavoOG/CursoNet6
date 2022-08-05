
using CursoNet6.Modelos;
using CursoNet6.Modelos.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace CursoNet6.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        //private readonly ApplicationDbContext _db;
        private readonly IProductoRepositorio _productoRepo;
        private readonly ICategoriaRepositorio _categoriaRepo;


        public HomeController(ILogger<HomeController> logger, IProductoRepositorio productoRepositorio, ICategoriaRepositorio categoriaRepositorio)
        {
            _logger = logger;
            _categoriaRepo = categoriaRepositorio;
            _productoRepo = productoRepositorio;
        }

        public IActionResult Index()
        {
            HomeVM homeVM = new HomeVM()
            {
                Productos = _productoRepo.ObtenerTodos(incluirPropiedades: "Categoria,TipoAplicacion"),
                Categorias = _categoriaRepo.ObtenerTodos()
            };
            return View(homeVM);
        }

        public IActionResult Detalle(int Id)
        {
            List<CarroCompra> carroCompras = new List<CarroCompra>();
            if (HttpContext.Session.Get<IEnumerable<CarroCompra>>(WC.SessionCarroCompras) != null &&
                HttpContext.Session.Get<IEnumerable<CarroCompra>>(WC.SessionCarroCompras).Count() > 0)
            {
                carroCompras = HttpContext.Session.Get<List<CarroCompra>>(WC.SessionCarroCompras);
            }


            DetalleVM detalleVM = new DetalleVM()
            {
                Producto = _productoRepo.ObtenerPrimero(m => m.Id == Id, incluirPropiedades: "Categoria,TipoAplicacion"),
                //_db.Producto.Include(m => m.Categoria).Include(m => m.TipoAplicacion).FirstOrDefault(m => m.Id == Id),
                ExisteEnCarro = carroCompras.Any(m => m.ProductoID == Id)
            };

            //foreach (var item in carroCompras)
            //{
            //    if (item.ProductoID == Id)
            //    {
            //        detalleVM.ExisteEnCarro = true;
            //    }
            //}

            return View(detalleVM);
        }


        [HttpPost, ActionName("Detalle")]
        public IActionResult DetallePost(int Id)
        {
            List<CarroCompra> carroCompras = new List<CarroCompra>();
            if (HttpContext.Session.Get<IEnumerable<CarroCompra>>(WC.SessionCarroCompras) != null &&
                HttpContext.Session.Get<IEnumerable<CarroCompra>>(WC.SessionCarroCompras).Count() > 0)
            {
                carroCompras = HttpContext.Session.Get<List<CarroCompra>>(WC.SessionCarroCompras);
            }
            carroCompras.Add(new CarroCompra() { ProductoID = Id });
            HttpContext.Session.Set(WC.SessionCarroCompras, carroCompras);
            return RedirectToAction("Index");
        }


        public IActionResult RemoverDeCarro(int Id)
        {
            List<CarroCompra> carroCompras = new List<CarroCompra>();
            if (HttpContext.Session.Get<IEnumerable<CarroCompra>>(WC.SessionCarroCompras) != null &&
                HttpContext.Session.Get<IEnumerable<CarroCompra>>(WC.SessionCarroCompras).Count() > 0)
            {
                carroCompras = HttpContext.Session.Get<List<CarroCompra>>(WC.SessionCarroCompras);
            }
            var productoARemover = carroCompras.SingleOrDefault(m => m.ProductoID == Id);
            if (productoARemover != null)
            {
                carroCompras.Remove(productoARemover);
            }

            HttpContext.Session.Set(WC.SessionCarroCompras, carroCompras);
            return RedirectToAction("Index");

        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
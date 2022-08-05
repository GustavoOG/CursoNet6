using CursoNet6.Modelos;
using CursoNet6.Modelos.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Text;

namespace CursoNet6.Controllers
{
    [Authorize]
    public class CarroController : Controller
    {

        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IEmailSender _emailSender;
        private readonly IProductoRepositorio _productoRepositorio;
        private readonly IUsuarioAplicacionRepositorio _usuarioAplicacionRepositorio;
        private readonly IOrdenRepositorio _ordenRepositorio;
        private readonly IOrdenDetalleRepositorio _ordenDetalleRepositorio;

        //BindProperty para utilizar propiedad en todo el controlador y no se pierdan sus propiedades
        [BindProperty]
        public ProductoUsuarioVM productoUsuarioVM { get; set; }

        public CarroController(IWebHostEnvironment webHostEnvironment, IEmailSender emailSender,
            IProductoRepositorio productoRepositorio,
            IUsuarioAplicacionRepositorio usuarioAplicacionRepositorio,
            IOrdenRepositorio ordenRepositorio,
            IOrdenDetalleRepositorio ordenDetalleRepositorio)
        {
            _webHostEnvironment = webHostEnvironment;
            _emailSender = emailSender;
            _productoRepositorio = productoRepositorio;
            _usuarioAplicacionRepositorio = usuarioAplicacionRepositorio;
            _ordenRepositorio = ordenRepositorio;
            _ordenDetalleRepositorio = ordenDetalleRepositorio;
        }

        public IActionResult Index()
        {
            List<CarroCompra> carroComprasList = new List<CarroCompra>();
            if (HttpContext.Session.Get<IEnumerable<CarroCompra>>(WC.SessionCarroCompras) != null &&
                HttpContext.Session.Get<IEnumerable<CarroCompra>>(WC.SessionCarroCompras).Count() > 0)
            {
                carroComprasList = HttpContext.Session.Get<List<CarroCompra>>(WC.SessionCarroCompras);
            }
            List<int> prodEnCarro = carroComprasList.Select(m => m.ProductoID).ToList();
            IEnumerable<Producto> prodList = _productoRepositorio.ObtenerTodos(m => prodEnCarro.Contains(m.Id));
            return View(prodList);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Index")]
        public IActionResult IndexPost()
        {
            return RedirectToAction(nameof(Resumen));
        }

        public IActionResult Resumen()
        {
            //Recuperar usuario logueado
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            List<CarroCompra> carroComprasList = new List<CarroCompra>();
            if (HttpContext.Session.Get<IEnumerable<CarroCompra>>(WC.SessionCarroCompras) != null &&
                HttpContext.Session.Get<IEnumerable<CarroCompra>>(WC.SessionCarroCompras).Count() > 0)
            {
                carroComprasList = HttpContext.Session.Get<List<CarroCompra>>(WC.SessionCarroCompras);
            }
            List<int> prodEnCarro = carroComprasList.Select(m => m.ProductoID).ToList();
            IEnumerable<Producto> prodLIst = _productoRepositorio.ObtenerTodos(m => prodEnCarro.Contains(m.Id));
            productoUsuarioVM = new ProductoUsuarioVM()
            {
                UsuarioAplicacion = _usuarioAplicacionRepositorio.ObtenerPrimero(m => m.Id == claim.Value),
                ProductoLista = prodLIst.ToList()
            };

            return View(productoUsuarioVM);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Resumen")]
        public async Task<IActionResult> ResumenPost(ProductoUsuarioVM productoUsuarioVM)
        {
            var claimsidentity = (ClaimsIdentity)User.Identity;
            var claim = claimsidentity.FindFirst(ClaimTypes.NameIdentifier);
            var rutaTemplete = Path.Combine(_webHostEnvironment.WebRootPath, "Templetes", "PlantillaOrden.html");
            var subject = "Nueva Orden";
            string HtmlBody = "";
            using (StreamReader sr = System.IO.File.OpenText(rutaTemplete))
            {
                HtmlBody = sr.ReadToEnd();
            }

            StringBuilder productoListaSB = new StringBuilder();
            foreach (var prod in productoUsuarioVM.ProductoLista)
            {
                productoListaSB.Append($" - Nombre: {prod.NombreProducto} <span style=\"font-size:14px;\"> (ID: {prod.Id})</span> <br />");
            }
            string messageBody = string.Format(HtmlBody,
                productoUsuarioVM.UsuarioAplicacion.NombreCompleto,
                productoUsuarioVM.UsuarioAplicacion.Email,
                productoUsuarioVM.UsuarioAplicacion.PhoneNumber, productoListaSB.ToString());

            await _emailSender.SendEmailAsync(WC.EmailAdmin, subject, messageBody);

            //Grabar la Orden y Detalle en la DB
            Orden orden = new Orden()
            {
                UsuarioAplicacionId = claim.Value,
                NombreCompleto = productoUsuarioVM.UsuarioAplicacion.NombreCompleto,
                Email = productoUsuarioVM.UsuarioAplicacion.Email,
                Telefono = productoUsuarioVM.UsuarioAplicacion.PhoneNumber,
                FechaOrden = DateTime.Now
            };
            _ordenRepositorio.Agregar(orden);
            _ordenRepositorio.Grabar();

            foreach (var prod in productoUsuarioVM.ProductoLista)
            {
                OrdenDetalle ordenDetalle = new OrdenDetalle()
                {
                    OrdenId = orden.Id,
                    ProductoId = prod.Id
                };
                _ordenDetalleRepositorio.Agregar(ordenDetalle);
            }
            _ordenDetalleRepositorio.Grabar();


            return RedirectToAction(nameof(Confirmacion));
        }

        public IActionResult Confirmacion()
        {
            HttpContext.Session.Clear();
            return View();
        }

        public IActionResult Remover(int Id)
        {
            List<CarroCompra> carroComprasList = new List<CarroCompra>();
            if (HttpContext.Session.Get<IEnumerable<CarroCompra>>(WC.SessionCarroCompras) != null &&
                HttpContext.Session.Get<IEnumerable<CarroCompra>>(WC.SessionCarroCompras).Count() > 0)
            {
                carroComprasList = HttpContext.Session.Get<List<CarroCompra>>(WC.SessionCarroCompras);
            }
            carroComprasList.Remove(carroComprasList.FirstOrDefault(m => m.ProductoID == Id));
            HttpContext.Session.Set(WC.SessionCarroCompras, carroComprasList);

            return RedirectToAction(nameof(Index));
        }
    }
}

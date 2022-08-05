
using CursoNet6.Modelos;
using CursoNet6.Modelos.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CursoNet6.Controllers
{
    [Authorize(Roles = WC.AdminRole)]
    public class ProductoController : Controller
    {
        private readonly IProductoRepositorio _prodRepo;

        private readonly IWebHostEnvironment _webHostEnvironment;

        public ProductoController(IProductoRepositorio prodRepo, IWebHostEnvironment webHostEnvironment)
        {
            this._prodRepo = prodRepo;
            this._webHostEnvironment = webHostEnvironment;
        }


        public IActionResult Index()
        {
            IEnumerable<Producto> lista = _prodRepo.ObtenerTodos(incluirPropiedades: "Categoria,TipoAplicacion");
            return View(lista);
        }

        //Get
        public IActionResult Upsert(int? Id)
        {
            //IEnumerable<SelectListItem> categoriaDropDown = _prodRepo.Categoria.Select(m => new SelectListItem()
            //{
            //    Text = m.NombreCategoria,
            //    Value = m.Id.ToString()
            //});

            ProductoVM productoVM = new ProductoVM()
            {
                Producto = new Producto(),
                CategoriaLista = _prodRepo.ObtenerTodosDownList(WC.CategoriaNombre),
                TipoAplicacionLista = _prodRepo.ObtenerTodosDownList(WC.TipoAplicacionNombre)
            };

            //ViewBag.categoriaDropDown = categoriaDropDown;
            Producto producto = new Producto();
            if (!Id.HasValue)
            {
                return View(productoVM);
            }
            else
            {
                productoVM.Producto = _prodRepo.Obtener(Id.GetValueOrDefault());
                if (producto == null)
                {
                    return NotFound();
                }
                return View(productoVM);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(ProductoVM productoVM)
        {
            if (ModelState.IsValid)
            {
                var files = HttpContext.Request.Form.Files;
                string webRootPath = _webHostEnvironment.WebRootPath;
                if (productoVM.Producto.Id == 0)
                {
                    //crear
                    string uploat = Path.Combine(webRootPath + WC.ImagenRuta);
                    string filenName = Guid.NewGuid().ToString();
                    string extension = Path.GetExtension(files[0].FileName);
                    using (var fileStream = new FileStream(Path.Combine(uploat, filenName + extension), FileMode.Create))
                    {
                        files[0].CopyTo(fileStream);
                    }
                    productoVM.Producto.ImagenUrl = filenName + extension;
                    _prodRepo.Agregar(productoVM.Producto);
                }
                else
                {
                    //Actualizar
                    var objProducto = _prodRepo.ObtenerPrimero(p => p.Id == productoVM.Producto.Id, isTracking: false);
                    if (files.Count > 0)
                    {//Cargar nueva imagen 
                        string uploat = Path.Combine(webRootPath + WC.ImagenRuta);
                        string filenName = Guid.NewGuid().ToString();
                        string extension = Path.GetExtension(files[0].FileName);

                        //borrar imagen anterior
                        var anteriorfile = Path.Combine(uploat, objProducto.ImagenUrl);
                        if (System.IO.File.Exists(anteriorfile))
                        {
                            System.IO.File.Delete(anteriorfile);
                        }
                        using (var fileStream = new FileStream(Path.Combine(uploat, filenName + extension), FileMode.Create))
                        {
                            files[0].CopyTo(fileStream);
                        }
                        productoVM.Producto.ImagenUrl = filenName + extension;
                    }// Caso contrario si no se carga nueva imagen 
                    else
                    {
                        productoVM.Producto.ImagenUrl = objProducto.ImagenUrl;
                    }
                    _prodRepo.Actualizar(productoVM.Producto);
                }
                _prodRepo.Grabar();
                return RedirectToAction("index");
            }// if ModelIsValid

            //Se llenan  nuevamente las listas si algo falla
            productoVM.CategoriaLista = _prodRepo.ObtenerTodosDownList(WC.CategoriaNombre);
            productoVM.TipoAplicacionLista = _prodRepo.ObtenerTodosDownList(WC.TipoAplicacionNombre);
            return View(productoVM);
        }


        //Get
        public IActionResult Eliminar(int? Id)
        {
            if (!Id.HasValue || Id == 0)
            {
                return NotFound();
            }
            else
            {
                Producto producto = _prodRepo.ObtenerPrimero(c => c.Id == Id, incluirPropiedades: "Categoria,TipoAplicacion");


                if (producto == null)
                {
                    return NotFound();
                }

                return View(producto);
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Eliminar(Producto producto)
        {

            if (producto == null)
            {
                return NotFound();
            }
            //Eliminar la imagen
            string uploat = Path.Combine(_webHostEnvironment.WebRootPath + WC.ImagenRuta);

            //borrar imagen anterior
            var anteriorfile = Path.Combine(uploat, producto.ImagenUrl);
            if (System.IO.File.Exists(anteriorfile))
            {
                System.IO.File.Delete(anteriorfile);
            }
            //borrar imagen anterior

            _prodRepo.Remover(producto);
            _prodRepo.Grabar();
            return RedirectToAction("Index");
        }

    }
}

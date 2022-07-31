
using CursoNet6.Modelos;
using CursoNet6.Modelos.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace CursoNet6.Controllers
{
    [Authorize(Roles = WC.AdminRole)]
    public class ProductoController : Controller
    {
        private readonly ApplicationDbContext _db;

        private readonly IWebHostEnvironment _webHostEnvironment;

        public ProductoController(ApplicationDbContext db, IWebHostEnvironment webHostEnvironment)
        {
            this._db = db;
            this._webHostEnvironment = webHostEnvironment;
        }


        public IActionResult Index()
        {
            IEnumerable<Producto> lista = _db.Producto.Include(c => c.Categoria).Include(c => c.TipoAplicacion);
            return View(lista);
        }

        //Get
        public IActionResult Upsert(int? Id)
        {
            //IEnumerable<SelectListItem> categoriaDropDown = _db.Categoria.Select(m => new SelectListItem()
            //{
            //    Text = m.NombreCategoria,
            //    Value = m.Id.ToString()
            //});

            ProductoVM productoVM = new ProductoVM()
            {
                Producto = new Producto(),
                CategoriaLista = _db.Categoria.Select(m => new SelectListItem()
                {
                    Text = m.NombreCategoria,
                    Value = m.Id.ToString()
                }),
                TipoAplicacionLista = _db.TipoAplicacion.Select(m => new SelectListItem()
                {
                    Text = m.Nombre,
                    Value = m.Id.ToString()
                })
            };

            //ViewBag.categoriaDropDown = categoriaDropDown;
            Producto producto = new Producto();
            if (!Id.HasValue)
            {
                return View(productoVM);
            }
            else
            {
                productoVM.Producto = _db.Producto.Find(Id.Value);
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
                    _db.Producto.Add(productoVM.Producto);
                }
                else
                {
                    //Actualizar
                    var objProducto = _db.Producto.AsNoTracking().FirstOrDefault(p => p.Id == productoVM.Producto.Id);
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
                    _db.Producto.Update(productoVM.Producto);
                }
                _db.SaveChanges();
                return RedirectToAction("index");
            }// if ModelIsValid

            //Se llenan  nuevamente las listas si algo falla
            productoVM.CategoriaLista = _db.Categoria.Select(m => new SelectListItem()
            {
                Text = m.NombreCategoria,
                Value = m.Id.ToString()
            });
            productoVM.TipoAplicacionLista = _db.TipoAplicacion.Select(m => new SelectListItem()
            {
                Text = m.Nombre,
                Value = m.Id.ToString()
            });

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
                Producto producto = _db.Producto.Include(c => c.Categoria).Include(c => c.TipoAplicacion)
                        .FirstOrDefault(c => c.Id == Id);

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

            _db.Producto.Remove(producto);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }

    }
}

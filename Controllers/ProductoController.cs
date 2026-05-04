using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using TiendaVirtualMVC.Data;
using TiendaVirtualMVC.Models;

namespace TiendaVirtualMVC.Controllers
{
    public class ProductoController : Controller
    {
        private readonly TiendaContext _context;

        public ProductoController(TiendaContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            if (HttpContext.Session.GetString("Usuario") == null)
            {
                return RedirectToAction("Index", "Login");
            }

            var productos = _context.Productos
                .Include(p => p.Categoria)
                .ToList();

            return View(productos);
        }

        public IActionResult Create()
        {
            if (HttpContext.Session.GetString("Usuario") == null)
                return RedirectToAction("Index", "Login");

            if (HttpContext.Session.GetString("Rol") != "admin")
                return RedirectToAction("Index");

            ViewBag.CategoriaId = new SelectList(_context.Categorias, "Id", "Nombre");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Producto producto, IFormFile imagen)
        {
            if (HttpContext.Session.GetString("Usuario") == null)
                return RedirectToAction("Index", "Login");

            ModelState.Remove("Categoria");

            if (ModelState.IsValid)
            {
                if (imagen != null && imagen.Length > 0)
                {
                    var nombreArchivo = Path.GetFileName(imagen.FileName);
                    var ruta = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", nombreArchivo);

                    using (var stream = new FileStream(ruta, FileMode.Create))
                    {
                        await imagen.CopyToAsync(stream);
                    }

                    producto.ImagenUrl = "/images/" + nombreArchivo;
                }

                _context.Productos.Add(producto);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.CategoriaId = new SelectList(_context.Categorias, "Id", "Nombre", producto.CategoriaId);
            return View(producto);
        }

        public IActionResult Edit(int id)
        {
            if (HttpContext.Session.GetString("Usuario") == null)
                return RedirectToAction("Index", "Login");

            var producto = _context.Productos.Find(id);
            if (producto == null) return NotFound();

            ViewBag.Categorias = _context.Categorias.ToList();
            return View(producto);
        }

        // MÉTODO EDIT MODIFICADO SEGÚN image_3ba79b.png
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Producto producto, IFormFile imagen)
        {
            if (HttpContext.Session.GetString("Usuario") == null)
                return RedirectToAction("Index", "Login");

            // Buscar el producto original en la base de datos
            var productoBD = _context.Productos.Find(producto.Id);
            if (productoBD == null)
            {
                return NotFound();
            }

            // Actualizar datos normales
            productoBD.Nombre = producto.Nombre;
            productoBD.Precio = producto.Precio;
            productoBD.Stock = producto.Stock;
            productoBD.CategoriaId = producto.CategoriaId;

            // Si sube nueva imagen
            if (imagen != null)
            {
                var carpeta = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images");

                // Verificar si la carpeta existe, si no, crearla
                if (!Directory.Exists(carpeta))
                {
                    Directory.CreateDirectory(carpeta);
                }

                var ruta = Path.Combine(carpeta, imagen.FileName);

                using (var stream = new FileStream(ruta, FileMode.Create))
                {
                    await imagen.CopyToAsync(stream);
                }

                // Actualizar la URL de la imagen en el objeto de la BD
                productoBD.ImagenUrl = "/images/" + imagen.FileName;
            }

            // Guardar cambios en la base de datos
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        public IActionResult Delete(int id)
        {
            if (HttpContext.Session.GetString("Usuario") == null)
                return RedirectToAction("Index", "Login");

            if (HttpContext.Session.GetString("Rol") != "admin")
                return RedirectToAction("Index");

            var producto = _context.Productos.Find(id);
            if (producto != null)
            {
                _context.Productos.Remove(producto);
                _context.SaveChanges();
            }
            return RedirectToAction("Index");
        }
    }
}
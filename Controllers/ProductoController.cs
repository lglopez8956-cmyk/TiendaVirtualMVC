using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TiendaVirtualMVC.Data;
using TiendaVirtualMVC.Models;
using Microsoft.AspNetCore.Http;
using System.Linq;

namespace TiendaVirtualMVC.Controllers
{
    public class ProductoController : Controller
    {
        private readonly TiendaContext _context;

        public ProductoController(TiendaContext context)
        {
            _context = context;
        }

        // Listado de productos
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

        // --- FORMULARIO PARA CREAR ---
        public IActionResult Create()
        {
            if (HttpContext.Session.GetString("Usuario") == null)
                return RedirectToAction("Index", "Login");

            // Solo el admin puede crear
            if (HttpContext.Session.GetString("Rol") != "admin")
                return RedirectToAction("Index");

            // Pasamos la lista de categorías para el Select
            ViewBag.Categorias = _context.Categorias.ToList();
            return View();
        }

       
        [HttpPost]
        public IActionResult Create(Producto producto)
        {
            if (HttpContext.Session.GetString("Usuario") == null)
            {
                return RedirectToAction("Index", "Login");
            }

            ModelState.Remove("Categoria");

            if (ModelState.IsValid)
            {
                _context.Productos.Add(producto);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Categorias = _context.Categorias.ToList();
            return View(producto);
        }

        
        public IActionResult Edit(int id)
        {
            if (HttpContext.Session.GetString("Usuario") == null)
                return RedirectToAction("Index", "Login");

            var producto = _context.Productos.Find(id);
            if (producto == null) return NotFound();

            // Cargamos las categorías para el desplegable
            ViewBag.Categorias = _context.Categorias.ToList();

            return View(producto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Producto producto)
        {
            if (HttpContext.Session.GetString("Usuario") == null)
            {
                return RedirectToAction("Index", "Login");
            }

            // ELIMINAR VALIDACIÓN DE CATEGORÍA:
            // Esto evita que el guardado falle porque el objeto Categoria completo es nulo
            ModelState.Remove("Categoria");

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(producto);
                    _context.SaveChanges();
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Error al guardar en la base de datos: " + ex.Message);
                }
            }

            // Si hay error, recargamos las categorías para que el Select no salga vacío
            ViewBag.Categorias = _context.Categorias.ToList();
            return View(producto);
        }

        // --- ELIMINAR PRODUCTO ---
        public IActionResult Delete(int id)
        {
            if (HttpContext.Session.GetString("Usuario") == null)
                return RedirectToAction("Index", "Login");

            // Solo admin puede eliminar
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
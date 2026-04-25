using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore; 
using TiendaVirtualMVC.Data;
using TiendaVirtualMVC.Models;
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

        //FORMULARIO
        public IActionResult Create()
        {
            if (HttpContext.Session.GetString("Usuario") == null)
            {
                return RedirectToAction("Index", "Login");
            }
            ViewBag.Categorias = _context.Categorias.ToList();
            return View();
        }

        //GUARDAR PRODUCTO
        [HttpPost]
        public IActionResult Create(Producto producto)
        {
            if (HttpContext.Session.GetString("Usuario") == null)
            {
                return RedirectToAction("Index", "Login");
            }
            _context.Productos.Add(producto);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }

        //FOMULARIO DE EDICION
        public IActionResult Edit(int id)
        {
            if (HttpContext.Session.GetString("Usuario") == null)
            {
                return RedirectToAction("Index", "Login");
            }
            var producto = _context.Productos.Find(id);
            ViewBag.Categorias = _context.Categorias.ToList();

            return View(producto);
        }
        //Actualizar Producto
        [HttpPost]
        public IActionResult Edit(Producto producto)
        {
            if (HttpContext.Session.GetString("Usuario") == null)
            {
                return RedirectToAction("Index", "Login");
            }
            _context.Productos.Update(producto);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        //Eliminar Producto
        public IActionResult Delete(int id)
        {
            if (HttpContext.Session.GetString("Usuario") == null)
            {
                return RedirectToAction("Index", "Login");
            }
            var rol = HttpContext.Session.GetString("Rol");

            // SOLO ADMIN PUEDE ELIMINAR
            if (rol != "admin")
            {
                return RedirectToAction("Index");
            }
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
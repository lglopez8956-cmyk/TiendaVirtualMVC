using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TiendaVirtualMVC.Models;
using TiendaVirtualMVC.Data;
using Microsoft.EntityFrameworkCore;

namespace TiendaVirtualMVC.Controllers
{
    public class CategoriaController : Controller
    {
        private readonly TiendaContext _context;

        public CategoriaController(TiendaContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            if (HttpContext.Session.GetString("Usuario") == null)
            {
                return RedirectToAction("Index", "Login");
            }

            var categorias = await _context.Categorias.ToListAsync();
            return View(categorias);
        }

        public IActionResult Create()
        {
            var rol = HttpContext.Session.GetString("Rol")?.Trim();
            if (!string.Equals(rol, "Admin", StringComparison.OrdinalIgnoreCase))
            {
                return RedirectToAction("Index");
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Categoria categoria)
        {
            // Verificación de Admin (Ignorando mayúsculas)
            var rol = HttpContext.Session.GetString("Rol")?.Trim();
            if (!string.Equals(rol, "Admin", StringComparison.OrdinalIgnoreCase))
            {
                return RedirectToAction("Index");
            }

            if (ModelState.IsValid)
            {
                _context.Add(categoria);
                await _context.SaveChangesAsync(); // ESTA LÍNEA ES VITAL
                return RedirectToAction(nameof(Index));
            }

            // Si llegas aquí, es porque ModelState.IsValid fue FALSO
            return View(categoria);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            var rol = HttpContext.Session.GetString("Rol")?.Trim();
            if (!string.Equals(rol, "Admin", StringComparison.OrdinalIgnoreCase))
            {
                return RedirectToAction("Index");
            }

            if (id == null) return NotFound();
            var categoria = await _context.Categorias.FindAsync(id);
            if (categoria == null) return NotFound();

            return View(categoria);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Categoria categoria)
        {
            var rol = HttpContext.Session.GetString("Rol")?.Trim();
            if (!string.Equals(rol, "Admin", StringComparison.OrdinalIgnoreCase))
            {
                return RedirectToAction("Index");
            }

            if (id != categoria.Id) return NotFound();

            if (ModelState.IsValid)
            {
                _context.Update(categoria);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(categoria);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var rol = HttpContext.Session.GetString("Rol")?.Trim();
            if (!string.Equals(rol, "Admin", StringComparison.OrdinalIgnoreCase))
            {
                return RedirectToAction("Index");
            }

            var categoria = await _context.Categorias.FindAsync(id);
            if (categoria != null)
            {
                _context.Categorias.Remove(categoria);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
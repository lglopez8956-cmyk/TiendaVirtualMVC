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

        private bool ValidarSesion() => HttpContext.Session.GetString("Usuario") != null;
        private bool EsAdmin() => HttpContext.Session.GetString("Rol") == "admin";

        public async Task<IActionResult> Index()
        {
            if (!ValidarSesion()) return RedirectToAction("Index", "Login");

            var productos = await _context.Productos
                .Include(p => p.Categoria)
                .AsNoTracking()
                .ToListAsync();

            return View(productos);
        }

        public IActionResult Create()
        {
            if (!ValidarSesion()) return RedirectToAction("Index", "Login");
            if (!EsAdmin()) return RedirectToAction("Index");

            ViewBag.CategoriaId = new SelectList(_context.Categorias, "Id", "Nombre");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Producto producto, IFormFile imagen)
        {
            if (!ValidarSesion()) return RedirectToAction("Index", "Login");

            ModelState.Remove("Categoria");

            if (ModelState.IsValid)
            {
                if (imagen != null && imagen.Length > 0)
                {
                    var nombreArchivo = Guid.NewGuid().ToString() + Path.GetExtension(imagen.FileName);
                    var ruta = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", nombreArchivo);

                    using (var stream = new FileStream(ruta, FileMode.Create))
                    {
                        await imagen.CopyToAsync(stream);
                    }

                    producto.ImagenUrl = "/images/" + nombreArchivo;
                }

                _context.Productos.Add(producto);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewBag.CategoriaId = new SelectList(_context.Categorias, "Id", "Nombre", producto.CategoriaId);
            return View(producto);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (!ValidarSesion()) return RedirectToAction("Index", "Login");

            if (id == null) return NotFound();

            var producto = await _context.Productos.FindAsync(id);
            if (producto == null) return NotFound();

            // IMPORTANTE: El nombre "CategoriaId" debe coincidir con el asp-items de la vista
            ViewBag.CategoriaId = new SelectList(_context.Categorias, "Id", "Nombre", producto.CategoriaId);

            return View(producto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Producto producto, IFormFile imagen)
        {
            if (!ValidarSesion()) return RedirectToAction("Index", "Login");

            if (id != producto.Id) return NotFound();

            // QUITAMOS ESTAS DOS VALIDACIONES
            ModelState.Remove("Categoria");
            ModelState.Remove("imagen"); 

            if (ModelState.IsValid)
            {
                try
                {
                    var productoExistente = await _context.Productos.FindAsync(id);
                    if (productoExistente == null) return NotFound();

                    // Si el usuario subió una imagen nueva, la procesamos
                    if (imagen != null && imagen.Length > 0)
                    {
                        if (!string.IsNullOrEmpty(productoExistente.ImagenUrl))
                        {
                            var rutaAnterior = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", productoExistente.ImagenUrl.TrimStart('/'));
                            if (System.IO.File.Exists(rutaAnterior)) System.IO.File.Delete(rutaAnterior);
                        }

                        var nombreArchivo = Guid.NewGuid().ToString() + Path.GetExtension(imagen.FileName);
                        var ruta = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", nombreArchivo);

                        using (var stream = new FileStream(ruta, FileMode.Create))
                        {
                            await imagen.CopyToAsync(stream);
                        }

                        productoExistente.ImagenUrl = "/images/" + nombreArchivo;
                    }

                   
                    productoExistente.Nombre = producto.Nombre;
                    productoExistente.Precio = producto.Precio;
                    productoExistente.Stock = producto.Stock;
                    productoExistente.CategoriaId = producto.CategoriaId;

                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Productos.Any(e => e.Id == producto.Id)) return NotFound();
                    else throw;
                }
            }

            ViewBag.CategoriaId = new SelectList(_context.Categorias, "Id", "Nombre", producto.CategoriaId);
            return View(producto);
        }

        public async Task<IActionResult> Delete(int id)
        {
            if (!ValidarSesion()) return RedirectToAction("Index", "Login");
            if (!EsAdmin()) return RedirectToAction("Index");

            var producto = await _context.Productos.FindAsync(id);
            if (producto != null)
            {
                _context.Productos.Remove(producto);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
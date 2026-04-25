using Microsoft.AspNetCore.Mvc;
using TiendaVirtualMVC.Data;
using TiendaVirtualMVC.Models;
using System.Linq;

namespace TiendaVirtualMVC.Controllers
{
    public class UsuarioController : Controller
    {
        private readonly TiendaContext _context;

        public UsuarioController(TiendaContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            if (HttpContext.Session.GetString("Usuario") == null)
            {
                return RedirectToAction("Index", "Login");
            }
            var usuarios = _context.Usuarios.ToList();

            return View(usuarios);
        }

        //Guardar usuario

        public IActionResult Create()
        {
            if (HttpContext.Session.GetString("Usuario") == null)
            {
                return RedirectToAction("Index", "Login");
            }
            return View();
        }

        [HttpPost]
        public IActionResult Create(Usuario usuario)
        {
            if (HttpContext.Session.GetString("Usuario") == null)
            {
                return RedirectToAction("Index", "Login");
            }
            if (!ModelState.IsValid)
            {
                return View(usuario); // vuelve al formulario con errores
            }

            _context.Usuarios.Add(usuario);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult Edit(Usuario usuario)
        {
            if (HttpContext.Session.GetString("Usuario") == null)
            {
                return RedirectToAction("Index", "Login");
            }
            if (!ModelState.IsValid)
            {
                return View(usuario);
            }

            _context.Usuarios.Update(usuario);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }

        // Acción para Eliminar Usuario
        public IActionResult Delete(int id)
        {
            // 1. Verificar si está logueado
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("Usuario")))
            {
                return RedirectToAction("Index", "Login");
            }

            // 2. Verificar si es Admin
            var rol = HttpContext.Session.GetString("Rol");
            if (rol != "admin")
            {
                // Si no es admin, lo mandamos de vuelta a la lista sin hacer nada
                return RedirectToAction("Index");
            }

            // 3. Si pasó las pruebas, procedemos a borrar
            var usuario = _context.Usuarios.Find(id);
            if (usuario != null)
            {
                _context.Usuarios.Remove(usuario);
                _context.SaveChanges();
            }

            return RedirectToAction("Index");
        }
    }
}
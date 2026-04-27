using Microsoft.AspNetCore.Http; // Asegúrate de tener esta línea
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using TiendaVirtualMVC.Data;
using TiendaVirtualMVC.Helpers;
using TiendaVirtualMVC.Models;

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

        // --- CREAR USUARIO ---

        public IActionResult Create()
        {
            
            // Verificamos sesión y que sea ADMIN (según el taller)
            if (HttpContext.Session.GetString("Usuario") == null)
            {
                return RedirectToAction("Index", "Login");
            }

            if (HttpContext.Session.GetString("Rol") != "admin")
            {
                return RedirectToAction("Index");
            }

            return View();
        }

        [HttpPost]
        public IActionResult Create(Usuario usuario)
        {
            usuario.Clave = HashHelper.ObtenerHash(usuario.Clave);
            if (HttpContext.Session.GetString("Usuario") == null)
            {
                return RedirectToAction("Index", "Login");
            }

            if (!ModelState.IsValid)
            {
                return View(usuario);
            }

            _context.Usuarios.Add(usuario);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        // --- EDITAR USUARIO ---

        // ESTE ES EL MÉTODO QUE TE FALTABA (GET)
        public IActionResult Edit(int id)
        {
            if (HttpContext.Session.GetString("Usuario") == null)
            {
                return RedirectToAction("Index", "Login");
            }

            // Buscamos al usuario por su ID para llenar el formulario
            var usuario = _context.Usuarios.Find(id);

            if (usuario == null)
            {
                return NotFound();
            }

            return View(usuario);
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

        // --- ELIMINAR USUARIO ---

        public IActionResult Delete(int id)
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("Usuario")))
            {
                return RedirectToAction("Index", "Login");
            }

            var rol = HttpContext.Session.GetString("Rol");
            if (rol != "admin")
            {
                return RedirectToAction("Index");
            }

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
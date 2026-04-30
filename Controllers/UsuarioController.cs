using Microsoft.AspNetCore.Http;
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

        // --- LISTAR USUARIOS (Requiere ser Admin) ---
        public IActionResult Index()
        {
            // Solo entra si hay sesión iniciada
            if (HttpContext.Session.GetString("Usuario") == null)
            {
                return RedirectToAction("Index", "Login");
            }

            var usuarios = _context.Usuarios.ToList();
            return View(usuarios);
        }

        // --- CREAR USUARIO (Acceso Público / Registro) ---

        public IActionResult Create()
        {
            // Se eliminaron las validaciones de sesión y de rol admin 
            // para que cualquier visitante pueda registrarse.
            return View();
        }

        [HttpPost]
        public ActionResult Create(Usuario usuario)
        {
            if (ModelState.IsValid)
            {
                usuario.Clave = HashHelper.ObtenerHash(usuario.Clave);
                _context.Usuarios.Add(usuario);
                _context.SaveChanges();

                // Una vez creado, lo mandamos al Login para que inicie sesión
                return RedirectToAction("Index", "Login");
            }

            return View(usuario);
        }

        // --- EDITAR USUARIO (Requiere ser Admin) ---

        public IActionResult Edit(int id)
        {
            // Validación de seguridad
            if (HttpContext.Session.GetString("Usuario") == null)
            {
                return RedirectToAction("Index", "Login");
            }

            // Validación de rol (opcional según tu lógica, pero recomendable)
            if (HttpContext.Session.GetString("Rol") != "admin")
            {
                return RedirectToAction("Index", "Home");
            }

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

        // --- ELIMINAR USUARIO (Solo Admin) ---

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

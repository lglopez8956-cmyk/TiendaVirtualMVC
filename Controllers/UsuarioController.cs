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

        //Eliminar usuario
        public IActionResult Delete(int id)
        {
            if (HttpContext.Session.GetString("Usuario") == null)
            {
                return RedirectToAction("Index", "Login");
            }
            var usuario = _context.Usuarios.Find(id);

            _context.Usuarios.Remove(usuario);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }


    }
}

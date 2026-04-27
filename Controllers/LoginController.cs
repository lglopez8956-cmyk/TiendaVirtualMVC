using Microsoft.AspNetCore.Mvc;
using TiendaVirtualMVC.Data;

using TiendaVirtualMVC.Helpers;

namespace TiendaVirtualMVC.Controllers
{
    public class LoginController : Controller
    {
        private readonly TiendaContext _context;
        public LoginController(TiendaContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Index(string correo, string clave)
        {
            
            string claveHash = HashHelper.ObtenerHash(clave);

            
            var usuario = _context.Usuarios
                .FirstOrDefault(u => u.Correo == correo && u.Clave == claveHash);

            if (usuario != null)
            {
                HttpContext.Session.SetString("Usuario", usuario.Nombre);
                HttpContext.Session.SetString("Rol", usuario.Rol);

                return RedirectToAction("Index", "Home");
            }

            ViewBag.Error = "Credenciales incorrectas";
            return View();
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index");
        }
    }
}
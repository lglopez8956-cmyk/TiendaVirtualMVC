using Microsoft.AspNetCore.Mvc;
using TiendaVirtualMVC.Data;
using TiendaVirtualMVC.Helpers;
using Microsoft.AspNetCore.Http;

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
            // 1. Obtener el hash de la clave ingresada
            string claveHash = HashHelper.ObtenerHash(clave);

            // 2. Buscar el usuario en SQL Server
            var usuario = _context.Usuarios
                .FirstOrDefault(u => u.Correo == correo && u.Clave == claveHash);

            if (usuario != null)
            {
                // 3. Limpiar espacios en blanco que SQL Server puede añadir a los campos de texto
                string nombreUsuario = usuario.Nombre?.Trim() ?? "Usuario";
                string rolUsuario = usuario.Rol?.Trim() ?? "Invitado";

                // 4. Guardar en sesión
                HttpContext.Session.SetString("Usuario", nombreUsuario);
                HttpContext.Session.SetString("Rol", rolUsuario);

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
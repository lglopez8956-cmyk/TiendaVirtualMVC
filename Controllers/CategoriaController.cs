using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TiendaVirtualMVC.Models;

namespace TiendaVirtualMVC.Controllers
{
    public class CategoriaController : Controller
    {
        public IActionResult Index()
        {
            var categorias = new List<Categoria>
            {
                new Categoria {Nombre = "Tecnología", Descripcion = "Productos tecnológicos"},
                new Categoria {Nombre = "Ropa", Descripcion = "Prendas de vestir"},
                new Categoria {Nombre = "", Descripcion = "Sin nombre definido"}
            };

            return View(categorias);
        }
    }
}

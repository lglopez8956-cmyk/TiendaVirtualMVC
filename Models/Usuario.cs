using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace TiendaVirtualMVC.Models
{
    public class Usuario 
    {
        public int Id { get; set; }

        public string Nombre { get; set; }

        public string Correo { get; set; }

        public string Rol { get; set; }
        public string Celular { get; set; } 
    }
}

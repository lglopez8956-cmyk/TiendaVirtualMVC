using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace TiendaVirtualMVC.Models
{
    public class Usuario
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Nombre { get; set; }

        [Required]
        [StringLength(100)]
        public string Correo { get; set; }

        [Required]
        [StringLength(100)]
        public string Rol { get; set; }

        [Required]
        [RegularExpression(@"^3\d{9}$",
            ErrorMessage = "El celular debe Iniciar en 3 y debe terner 10 digitos")]
        public string celular { get; set; }

        public string Clave { get; set; }
    }
}
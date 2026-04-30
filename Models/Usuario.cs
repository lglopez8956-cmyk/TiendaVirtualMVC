using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel; 

namespace TiendaVirtualMVC.Models
{
    public class Usuario
    {
        [Key] 
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio")]
        [StringLength(100, ErrorMessage = "El nombre no puede exceder los 100 caracteres")]
        [Display(Name = "Nombre Completo")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "El correo es obligatorio")]
        [EmailAddress(ErrorMessage = "Correo inválido")]
        [Display(Name = "Correo Electrónico")]
        public string Correo { get; set; }

        [Required(ErrorMessage = "El rol es obligatorio")]
        [Display(Name = "Rol de Usuario")]
        public string Rol { get; set; }

        [Required(ErrorMessage = "El celular es obligatorio")]
        [RegularExpression(@"^3\d{9}$", ErrorMessage = "El celular debe iniciar con 3 y tener 10 dígitos")]
        [Display(Name = "Número de Celular")]
        public string Celular { get; set; }

        [Required(ErrorMessage = "La clave es obligatoria")]
        [MinLength(4, ErrorMessage = "Mínimo 4 caracteres")]
        [DataType(DataType.Password)] 
        [Display(Name = "Contraseña")]
        public string Clave { get; set; }
    }
}
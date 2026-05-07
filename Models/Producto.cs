using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TiendaVirtualMVC.Models
{
    public class Producto
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio")]
        [StringLength(10000)]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "El precio es obligatorio")]
        [Range(0, 100000000, ErrorMessage = "El precio debe estar entre 0 y 100.000.000")]
        public double Precio { get; set; }

        [Required]
        [Range(0, 10000, ErrorMessage = "El stock debe estar entre 0 y 10.000")]
        public int Stock { get; set; }

        [Required]
        public int CategoriaId { get; set; }

        [ForeignKey("CategoriaId")]
        [ValidateNever]
        public virtual Categoria? Categoria { get; set; }

        public string? ImagenUrl { get; set; }

        [NotMapped]
        public IFormFile? ImagenFile { get; set; }

        public double CalcularValorInventario()
        {
            return Precio * Stock;
        }

        public bool TieneStock()
        {
            return Stock > 0;
        }
    }
}
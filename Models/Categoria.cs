using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TiendaVirtualMVC.Models
{
    public class Categoria
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Nombre { get; set; }

        [Required]
        [StringLength(500)]
        [DataType(DataType.Text)]
        public string Descripcion { get; set; }

        // Esta es la relación que permite saber si hay productos
        public virtual ICollection<Producto> Productos { get; set; } = new List<Producto>();
    }
}
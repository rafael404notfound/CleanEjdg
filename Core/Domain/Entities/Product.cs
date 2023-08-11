using CleanEjdg.Core.Domain.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanEjdg.Core.Domain.Entities
{
    public class Product : BaseEntity
    {
        [Required]
        [MaxLength(30)]
        public string Name { get; set; } = string.Empty;
        [Required]
        //[Range(0.1, int.MaxValue, ErrorMessage = "El precio no puede ser negativo ")]
        public int Price { get; set; }
        [Required]
        public string Category { get; set; } = string.Empty;
        [Required]
        [MaxLength(150)]
        public string Description { get; set; } = string.Empty;
        public List<ProductPhoto> Photos { get; set; } = new List<ProductPhoto>();
    }
}

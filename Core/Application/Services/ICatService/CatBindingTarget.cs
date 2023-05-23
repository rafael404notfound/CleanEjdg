using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace CleanEjdg.Core.Application.Services
{
    public class CatBindingTarget
    {
        // Why does model binding validation only work when the property is nullable????

        [Required]
        public string Name { get; set; } = string.Empty;
        [Required]
        public DateTime? DateOfBirth { get; set; }
        [Required]
        public bool? IsVaccinated { get; set; }
        [Required]
        public bool? HasChip { get; set; }
        [Required]
        public bool IsSterilized { get; set; }

        public Cat ToCat() => new Cat()
        {
            Name = this.Name,
            DateOfBirth = this.DateOfBirth ?? new DateTime(),
            IsVaccinated = this.IsVaccinated ?? false,
            HasChip = this.HasChip ?? false,
            IsSterilized = this.IsSterilized,
        };
    }
}

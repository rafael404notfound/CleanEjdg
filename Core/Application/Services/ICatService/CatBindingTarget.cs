using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using CleanEjdg.Core.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Components.Forms;

namespace CleanEjdg.Core.Application.Services
{
    public class CatBindingTarget
    {
        // Why does model binding validation only work when the property is nullable????

        [Required]
        public virtual string Name { get; set; } = string.Empty;
        [Required]
        public virtual DateTime? DateOfBirth { get; set; }
        [Required]
        public virtual bool? IsVaccinated { get; set; }
        [Required]
        public virtual bool? HasChip { get; set; }
        [Required]
        public virtual bool? IsSterilized { get; set; }
        [Required]
        public List<CatPhoto> Photos { get; set; } = new List<CatPhoto>();
        public List<IBrowserFile>? Files { get; set; }

        public virtual Cat ToCat() => new Cat()
        {
            Name = this.Name,
            DateOfBirth = DateTime.SpecifyKind(this.DateOfBirth ?? new DateTime(), DateTimeKind.Utc),
            IsVaccinated = this.IsVaccinated ?? false,
            HasChip = this.HasChip ?? false,
            IsSterilized = this.IsSterilized ?? false,
            Photos = this.Photos,
            Files = this.Files
        };
    }
}

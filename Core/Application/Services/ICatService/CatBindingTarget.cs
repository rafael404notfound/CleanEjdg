using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanEjdg.Core.Application.Services
{
    public class CatBindingTarget
    {
        public string? Name { get; set; } = string.Empty;
        public DateTime DateOfBirth { get; set; }
        public bool IsVaccinated { get; set; }
        public bool HasChip { get; set; }
        public bool IsSterilized { get; set; }

        public Cat ToCat() => new Cat()
        {
            Name = this.Name,
            DateOfBirth = this.DateOfBirth,
            IsVaccinated = this.IsVaccinated,
            HasChip = this.HasChip,
            IsSterilized = this.IsSterilized
        };
    }
}

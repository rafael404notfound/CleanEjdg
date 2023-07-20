using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CleanEjdg.Core.Domain.Common;

namespace CleanEjdg.Core.Domain.Entities
{
    public class CatPhoto : BaseEntity
    {
        public Byte[]? Bytes { get; set; }
        public string Description { get; set; } = String.Empty;
        public string FileExtension { get; set; } = String.Empty;
        public decimal Size { get; set; }
        public int CatId { get; set; }
    }
}

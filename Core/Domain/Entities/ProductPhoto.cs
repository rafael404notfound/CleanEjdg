using CleanEjdg.Core.Domain.Common;

namespace CleanEjdg.Core.Domain.Entities
{
    public class ProductPhoto : BaseEntity
    {
        public Byte[]? Bytes { get; set; }
        public string Description { get; set; } = String.Empty;
        public string FileExtension { get; set; } = String.Empty;
        public decimal Size { get; set; }
        public int ProductId { get; set; }
    }
}

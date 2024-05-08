namespace TechnicalInterview.OmniaRetail.Application.Domain
{
    public class Product
    {
        public Guid Id { get; set; } = default!;
        public string Names { get; set; } = default!;
        public string? Details { get; set; }

        public string Brand { get; set; } = default!;

        public virtual ICollection<ProductGroup> ProductGroups { get; set; } = default!;
    }
}

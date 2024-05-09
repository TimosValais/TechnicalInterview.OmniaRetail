namespace TechnicalInterview.OmniaRetail.Application.Models
{
    public class Product
    {
        public required Guid Id { get; init; } = Guid.NewGuid();
        public string Name { get; set; } = default!;
        public string? Description { get; set; }

        public string Brand { get; set; } = default!;

        public List<RetailerProductPrice> RetailerProductPrices { get; } = [];
        public List<ProductGroup> ProductGroups { get; } = [];
    }
}

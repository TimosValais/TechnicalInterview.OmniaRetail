namespace TechnicalInterview.OmniaRetail.Application.Models
{
    public class ProductGroup
    {
        public required Guid Id { get; init; } = Guid.NewGuid();
        public string Name { get; set; } = default!;

        public List<Product> Products { get; } = [];
    }
}

namespace TechnicalInterview.OmniaRetail.Application.Models
{
    public class Retailer
    {
        public required Guid Id { get; init; } = Guid.NewGuid();
        public string Name { get; set; } = default!;
        public string Address { get; set; } = default!;
        public List<RetailerProductPrice> RetailerProductPrices { get; } = [];
    }
}

namespace TechnicalInterview.OmniaRetail.Application.Domain
{
    public class Retailer
    {
        public Guid Id { get; set; } = default!;
        public string Name { get; set; } = default!;
        public string Address { get; set; } = default!;
        public ICollection<ProductRetailerPrice> ProductRetailerPrices { get; set; } = default!;
    }
}

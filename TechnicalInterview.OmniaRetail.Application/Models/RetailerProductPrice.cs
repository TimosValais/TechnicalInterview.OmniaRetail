namespace TechnicalInterview.OmniaRetail.Application.Models
{
    public class RetailerProductPrice
    {
        public Guid ProductId { get; set; }
        public Guid RetailerId { get; set; }
        public Product Product { get; set; } = null!;
        public Retailer Retailer { get; set; } = null!;
        public int Price { get; set; }

    }
}

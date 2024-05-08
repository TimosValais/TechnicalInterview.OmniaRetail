namespace TechnicalInterview.OmniaRetail.Application.Domain
{
    public class ProductRetailerPrice
    {
        public Guid ProductId { get; set; } = default!;
        public Guid RetailerId { get; set; } = default!;

        public int Price { get; set; }

        public virtual Product Product { get; set; } = default!;
        public virtual Retailer Retailer { get; set; } = default!;

    }
}

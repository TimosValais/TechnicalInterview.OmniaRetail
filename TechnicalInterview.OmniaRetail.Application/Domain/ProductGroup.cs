namespace TechnicalInterview.OmniaRetail.Application.Domain
{
    public class ProductGroup
    {
        public Guid Id { get; set; } = default!;
        public string Name { get; set; } = default!;

        public virtual ICollection<Product> Products { get; set; } = default!;
    }
}

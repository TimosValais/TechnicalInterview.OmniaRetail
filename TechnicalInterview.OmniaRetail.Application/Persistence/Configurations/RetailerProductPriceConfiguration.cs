using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TechnicalInterview.OmniaRetail.Application.Models;

namespace TechnicalInterview.OmniaRetail.Application.Persistence.Configurations
{
    internal class RetailerProductPriceConfiguration : IEntityTypeConfiguration<RetailerProductPrice>
    {
        public void Configure(EntityTypeBuilder<RetailerProductPrice> builder)
        {
            //adding composite key because each retailer needs to have one price on a product
            builder.HasKey(rp => new { rp.ProductId, rp.RetailerId });
        }
    }
}

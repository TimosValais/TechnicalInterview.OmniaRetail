using TechincalInterview.OmniaRetail.Contracts.Requests;
using TechnicalInterview.OmniaRetail.Application.Domain;

namespace TechnicalInterview.OmniaRetail.Api.Mappings
{
    public static class ContractMapings
    {
        public static ProductRetailerPrice MapToProductRetailerPrice(this UpdateProductPriceRequest request, Guid retailerId)
        {
            return new ProductRetailerPrice
            {
                ProductId = request.ProductId,
                RetailerId = retailerId,
                Price = request.Price,
            };
        }
    }
}

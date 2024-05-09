using TechincalInterview.OmniaRetail.Contracts.Requests;
using TechnicalInterview.OmniaRetail.Application.Models;

namespace TechnicalInterview.OmniaRetail.Api.Mappings
{
    public static class ContractMapings
    {
        public static RetailerProductPrice MapToProductRetailerPrice(this UpdateProductPriceRequest request, Guid retailerId)
        {
            return new RetailerProductPrice
            {
                ProductId = request.ProductId,
                RetailerId = retailerId,
                Price = request.Price,
            };
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using TechincalInterview.OmniaRetail.Contracts;
using TechnicalInterview.OmniaRetail.Api.Endpoints.Internal;
using TechnicalInterview.OmniaRetail.Application.Domain;
using TechnicalInterview.OmniaRetail.Application.Services;

namespace TechnicalInterview.OmniaRetail.Api.Endpoints
{
    public class ProductEndpoints : IEndpoints
    {
        public static void DefineEndPoints(IEndpointRouteBuilder app)
        {
            app.MapGet(ApiEndpointsConstants.Product.Get, GetProductById);
            app.MapGet(ApiEndpointsConstants.Product.GetPrices, GetProductPricesById);
            app.MapGet(ApiEndpointsConstants.Product.GetHighestPrice, GetProductHighestPriceById);
            app.MapGet(ApiEndpointsConstants.Product.GetPriceRecommendations, GetPriceRecomendationByProductId);
        }



        private static async Task<IResult> GetProductById([FromRoute] Guid productGuid, IProductService productService, CancellationToken cancellationToken)
        {
            Product? product = await productService.GetByIdAsync(productGuid, cancellationToken);
            return product is not null ? Results.Ok(product) : Results.NotFound($"Product with Id {productGuid} was not found");
        }
        private static async Task<IResult> GetProductPricesById([FromRoute] Guid productGuid, IProductService productService, CancellationToken cancellationToken)
        {
            IEnumerable<int> productPrices = await productService.GetProductPricesAsync(productGuid, cancellationToken);
            return Results.Ok(productPrices);
        }

        private static async Task<IResult> GetProductHighestPriceById([FromRoute] Guid productGuid, IProductService productService, [FromQuery] string? tier, CancellationToken cancellationToken)
        {
            if (!Enum.TryParse(tier, out PriceTier priceTier))
            {
                priceTier = PriceTier.Tier1;
            }
            int highestPrice = await productService.GetPriceRecommendationByIdAsync(productGuid, priceTier, cancellationToken);
            return highestPrice > 0 ? Results.Ok(highestPrice) : Results.NotFound("Couldn't find highest price for this product");
        }
        private static async Task<IResult> GetPriceRecomendationByProductId([FromRoute] Guid productGuid, IProductService productService, [FromQuery] string? tier, CancellationToken cancellationToken)
        {
            if (!Enum.TryParse(tier, out PriceTier priceTier))
            {
                priceTier = PriceTier.Tier3;
            }
            int priceRecommendation = await productService.GetPriceRecommendationByIdAsync(productGuid, priceTier, cancellationToken);
            return priceRecommendation > 0 ? Results.Ok(priceRecommendation) : Results.Problem("Couldn't recommend price for this product");

        }
    }
}

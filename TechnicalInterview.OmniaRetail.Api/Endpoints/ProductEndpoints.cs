using Microsoft.AspNetCore.Mvc;
using TechincalInterview.OmniaRetail.Contracts;
using TechnicalInterview.OmniaRetail.Api.Endpoints.Internal;
using TechnicalInterview.OmniaRetail.Application.Models;
using TechnicalInterview.OmniaRetail.Application.Services;

namespace TechnicalInterview.OmniaRetail.Api.Endpoints
{
    public class ProductEndpoints : IEndpoints
    {
        public static void DefineEndPoints(IEndpointRouteBuilder app)
        {
            app.MapGet(ApiEndpointsConstants.Product.GetAll, GetAllProducts);
            app.MapGet(ApiEndpointsConstants.Product.Get, GetProductById);
            app.MapGet(ApiEndpointsConstants.Product.GetPrices, GetProductPricesById);
            app.MapGet(ApiEndpointsConstants.Product.GetHighestPrice, GetProductHighestPriceById);
            app.MapGet(ApiEndpointsConstants.Product.GetPriceRecommendations, GetPriceRecomendationByProductId);

        }

        private static async Task<IResult> GetAllProducts(IProductService productService, CancellationToken cancellationToken)
        {
            IEnumerable<Product> products = await productService.ListAsync(cancellationToken);
            return Results.Ok(products);
        }

        private static async Task<IResult> GetProductById(Guid id, IProductService productService, CancellationToken cancellationToken)
        {
            Product? product = await productService.GetByIdAsync(id, cancellationToken);
            return product is not null ? Results.Ok(product) : Results.NotFound($"Product with Id {id} was not found");
        }
        private static async Task<IResult> GetProductPricesById(Guid id, IProductService productService, CancellationToken cancellationToken)
        {
            IEnumerable<int> productPrices = await productService.GetProductPricesAsync(id, cancellationToken);
            return Results.Ok(productPrices);
        }

        private static async Task<IResult> GetProductHighestPriceById(Guid id, IProductService productService, CancellationToken cancellationToken)
        {
            int highestPrice = await productService.GetProductHighestTier1PriceAsync(id, cancellationToken);
            return highestPrice > 0 ? Results.Ok(highestPrice) : Results.NotFound("Couldn't find highest price for this product");
        }
        private static async Task<IResult> GetPriceRecomendationByProductId(Guid id, IProductService productService, [FromQuery] string? tier, CancellationToken cancellationToken)
        {
            if (!Enum.TryParse(tier, out PriceTier priceTier))
            {
                priceTier = PriceTier.Tier3;
            }
            int priceRecommendation = await productService.GetPriceRecommendationByIdAsync(id, priceTier, cancellationToken);
            return priceRecommendation > 0 ? Results.Ok(priceRecommendation) : Results.Problem("Couldn't recommend price for this product");

        }
    }
}

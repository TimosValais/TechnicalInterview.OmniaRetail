using Microsoft.AspNetCore.Mvc;
using TechincalInterview.OmniaRetail.Contracts;
using TechnicalInterview.OmniaRetail.Api.Endpoints.Internal;
using TechnicalInterview.OmniaRetail.Api.Mappings;
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
            app.MapGet(ApiEndpointsConstants.ProductGroup.GetAll, GetAllProductGroups);

        }


        private static async Task<IResult> GetAllProducts(IProductService productService, CancellationToken cancellationToken)
        {
            IEnumerable<Product> products = await productService.ListAsync(cancellationToken);
            return Results.Ok(products.MapToProductResponses());
        }

        private static async Task<IResult> GetProductById(Guid id, IProductService productService, CancellationToken cancellationToken)
        {
            Product? product = await productService.GetByIdAsync(id, cancellationToken);
            return product is not null ? Results.Ok(product.MapToProductResponse()) : Results.NotFound($"Product with Id {id} was not found");
        }
        private static async Task<IResult> GetProductPricesById(Guid id, IProductService productService, CancellationToken cancellationToken)
        {
            IEnumerable<int> productPrices = await productService.GetProductPricesAsync(id, cancellationToken);
            return Results.Ok(ContractMapings.MapToPriceResponses(productPrices));
        }

        private static async Task<IResult> GetProductHighestPriceById(Guid id, IProductService productService, CancellationToken cancellationToken)
        {
            int highestPrice = await productService.GetProductHighestTier1PriceAsync(id, cancellationToken);
            return highestPrice > 0 ? Results.Ok(ContractMapings.MapToPriceResponse(highestPrice)) : Results.NotFound("Couldn't find highest price for this product");
        }
        private static async Task<IResult> GetPriceRecomendationByProductId(Guid id, IProductService productService, [FromQuery] string? tier, CancellationToken cancellationToken)
        {
            if (!Enum.TryParse(tier, out PriceTier priceTier))
            {
                priceTier = PriceTier.Tier3;
            }
            int priceRecommendation = await productService.GetPriceRecommendationByIdAsync(id, priceTier, cancellationToken);
            return priceRecommendation > 0 ? Results.Ok(ContractMapings.MapToPriceResponse(priceRecommendation)) : Results.Problem("Couldn't recommend price for this product");

        }

        private static async Task<IResult> GetAllProductGroups(IProductService productService, CancellationToken cancellationToken)
        {
            IEnumerable<ProductGroup> allGroups = await productService.ListProductGroupsAsync(cancellationToken);
            return Results.Ok(allGroups.MapToProductGroupResponses());
        }

    }
}

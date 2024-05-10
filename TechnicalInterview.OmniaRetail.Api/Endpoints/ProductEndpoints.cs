using Microsoft.AspNetCore.Mvc;
using TechincalInterview.OmniaRetail.Contracts;
using TechincalInterview.OmniaRetail.Contracts.Responses;
using TechnicalInterview.OmniaRetail.Api.Endpoints.Internal;
using TechnicalInterview.OmniaRetail.Api.Mappings;
using TechnicalInterview.OmniaRetail.Application.Models;
using TechnicalInterview.OmniaRetail.Application.Services;

namespace TechnicalInterview.OmniaRetail.Api.Endpoints
{
    public class ProductEndpoints : IEndpoints
    {
        private const int cacheExpirationSeconds = 15;
        private const string GroupName = "Product Endpoints";
        public static void DefineEndPoints(IEndpointRouteBuilder app)
        {
            app.MapGet(ApiEndpointsConstants.Product.GetAll, GetAllProducts)
                .CacheOutput(x => x.Expire(TimeSpan.FromSeconds(cacheExpirationSeconds)))
                .WithName("Get All Products")
                .Produces<IEnumerable<ProductResponse>>(200);
            app.MapGet(ApiEndpointsConstants.Product.Get, GetProductById)
                .CacheOutput(x => x.SetVaryByRouteValue("id")
                .Expire(TimeSpan.FromSeconds(cacheExpirationSeconds)))
                .WithName("Get Product By Id")
                .Produces<ProductResponse>(200)
                .Produces(404);

            app.MapGet(ApiEndpointsConstants.Product.GetPrices, GetProductPricesById)
                .CacheOutput(x => x.SetVaryByRouteValue("id")
                .Tag(OutputCacheConstants.CacheTags.Prices)
                .Expire(TimeSpan.FromSeconds(cacheExpirationSeconds)))
                .RequireAuthorization()
                .WithName("Get Prices By Product Id")
                .Produces<IEnumerable<ProductPriceResponse>>(200);

            app.MapGet(ApiEndpointsConstants.Product.GetHighestPrice, GetProductHighestPriceById)
                .CacheOutput(x => x.SetVaryByRouteValue("id")
                .Tag(OutputCacheConstants.CacheTags.Prices)
                .Expire(TimeSpan.FromSeconds(cacheExpirationSeconds)))
                .RequireAuthorization()
                .WithName("Get Product Highest Price")
                .Produces<ProductPriceResponse>(200)
                .Produces(404);

            app.MapGet(ApiEndpointsConstants.Product.GetPriceRecommendations, GetPriceRecomendationByProductId)
                .CacheOutput(x => x.SetVaryByRouteValue("id")
                .Tag(OutputCacheConstants.CacheTags.Prices)
                .Expire(TimeSpan.FromSeconds(cacheExpirationSeconds)))
                .RequireAuthorization()
                .WithName("Get Price Recomendation For A Product")
                .Produces<ProductPriceResponse>(200)
                .Produces(404);

            app.MapGet(ApiEndpointsConstants.ProductGroup.GetAll, GetAllProductGroups)
                .CacheOutput(x => x.Expire(TimeSpan.FromSeconds(cacheExpirationSeconds)));

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
            return priceRecommendation > 0 ? Results.Ok(ContractMapings.MapToPriceResponse(priceRecommendation)) : Results.NotFound("Couldn't recommend price for this product");

        }

        private static async Task<IResult> GetAllProductGroups(IProductService productService, CancellationToken cancellationToken)
        {
            IEnumerable<ProductGroup> allGroups = await productService.ListProductGroupsAsync(cancellationToken);
            return Results.Ok(allGroups.MapToProductGroupResponses());
        }

    }
}

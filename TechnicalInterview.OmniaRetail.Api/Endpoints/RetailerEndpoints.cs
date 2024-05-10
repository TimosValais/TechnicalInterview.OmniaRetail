using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;
using TechincalInterview.OmniaRetail.Contracts.Requests;
using TechnicalInterview.OmniaRetail.Api.Auth;
using TechnicalInterview.OmniaRetail.Api.Endpoints.Internal;
using TechnicalInterview.OmniaRetail.Api.Mappings;
using TechnicalInterview.OmniaRetail.Application.Models;
using TechnicalInterview.OmniaRetail.Application.Services;

namespace TechnicalInterview.OmniaRetail.Api.Endpoints
{
    public class RetailerEndpoints : IEndpoints
    {
        public static void DefineEndPoints(IEndpointRouteBuilder app)
        {
            app.MapGet(ApiEndpointsConstants.ProductGroup.GetCompetitors, GetCompetitorsByProductGroupId);
            app.MapPut(ApiEndpointsConstants.Product.UpdatePrices, UpdateRetailerProductPrices);
        }
        [Authorize(Policy = AuthConstants.RetailerAdminPolicyName)]
        private static async Task<IResult> GetCompetitorsByProductGroupId(Guid productGroupId, IRetailerService retailerService, HttpContext context, CancellationToken cancellationToken)
        {
            Guid? retailerId = context.GetRetailerId();
            if (retailerId is null)
            {
                return Results.Unauthorized();
            }
            IEnumerable<Retailer> competitors = await retailerService.GetCompetitorsByProductGroupIdAsync(productGroupId, (Guid)retailerId, cancellationToken);
            return Results.Ok(competitors.MapToCompetitorResponses());
        }
        [Authorize(Policy = AuthConstants.RetailerAdminPolicyName)]
        private static async Task<IResult> UpdateRetailerProductPrices([FromBody] ICollection<UpdateProductPriceRequest> productPrices,
                                                                        IRetailerService retailerService,
                                                                        HttpContext context, IOutputCacheStore cache,
                                                                        CancellationToken cancellationToken)
        {
            Guid? retailerId = context.GetRetailerId();
            if (retailerId is null)
            {
                return Results.Unauthorized();
            }
            List<RetailerProductPrice> productRetailerPrices = [];
            foreach (UpdateProductPriceRequest productPrice in productPrices)
            {
                productRetailerPrices.Add(productPrice.MapToProductRetailerPrice((Guid)retailerId));
            }
            bool updated = await retailerService.UpdatePricesAsync(productRetailerPrices, cancellationToken);
            await cache.EvictByTagAsync(OutputCacheConstants.CacheTags.Prices, cancellationToken);
            return updated ? Results.Ok(productRetailerPrices.MapToProductPriceResponses()) : Results.NotFound("None of the products were updated");
        }

    }
}

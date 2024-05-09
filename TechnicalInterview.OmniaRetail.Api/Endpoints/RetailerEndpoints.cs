using Microsoft.AspNetCore.Mvc;
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

        private static async Task<IResult> GetCompetitorsByProductGroupId([FromRoute] Guid productGroupId, IRetailerService retailerService, CancellationToken cancellationToken)
        {
            IEnumerable<Retailer> competitors = await retailerService.GetCompetitorsByProductGroupIdAsync(productGroupId, cancellationToken);
            return Results.Ok(competitors);
        }
        private static async Task<IResult> UpdateRetailerProductPrices([FromBody] ICollection<UpdateProductPriceRequest> productPrices, IRetailerService retailerService, HttpContext context, CancellationToken cancellationToken)
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
            return updated ? Results.Ok(productRetailerPrices) : Results.BadRequest();
        }

    }
}

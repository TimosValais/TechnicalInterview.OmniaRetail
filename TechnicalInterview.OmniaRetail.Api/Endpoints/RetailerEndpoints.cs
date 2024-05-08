using Microsoft.AspNetCore.Mvc;
using TechnicalInterview.OmniaRetail.Api.Endpoints.Internal;
using TechnicalInterview.OmniaRetail.Application.Domain;
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
        private static async Task<IResult> UpdateRetailerProductPrices()
        {
            throw new NotImplementedException();
        }

    }
}

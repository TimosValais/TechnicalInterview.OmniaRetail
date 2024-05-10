using Microsoft.AspNetCore.Mvc;
using TechincalInterview.OmniaRetail.Contracts.Requests;
using TechnicalInterview.OmniaRetail.Api.Auth;
using TechnicalInterview.OmniaRetail.Api.Endpoints.Internal;

namespace TechnicalInterview.OmniaRetail.Api.Endpoints
{
    public class IdentityEndpoints : IEndpoints
    {
        public static void DefineEndPoints(IEndpointRouteBuilder app)
        {
            app.MapPost("identity/token", GenerateToken);
        }

        private static async Task<IResult> GenerateToken([FromBody] TokenGenerationRequest tokenRequest, IIdentityService identityService)
        {
            string jwt = await identityService.GenerateToken(tokenRequest);
            return !String.IsNullOrEmpty(jwt) ? Results.Ok(jwt) : Results.BadRequest();
        }
    }
}

using TechincalInterview.OmniaRetail.Contracts.Requests;

namespace TechnicalInterview.OmniaRetail.Api.Auth
{
    //this is for demo purposes only, this would be its own service for the token generation
    public interface IIdentityService
    {
        Task<string> GenerateToken(TokenGenerationRequest tokenRequest);
    }
}

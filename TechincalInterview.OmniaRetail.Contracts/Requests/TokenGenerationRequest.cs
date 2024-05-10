namespace TechincalInterview.OmniaRetail.Contracts.Requests
{
    public record TokenGenerationRequest(Guid RetailerId, Dictionary<string, object>? CustomClaims);
}

namespace TechincalInterview.OmniaRetail.Contracts.Requests
{
    public record UpdateProductPriceRequest(Guid ProductId, int Price);
}

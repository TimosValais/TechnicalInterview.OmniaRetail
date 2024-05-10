namespace TechincalInterview.OmniaRetail.Contracts.Responses
{
    //this is not needed yet but if for example currency was actually implemented and not mocked
    //this would be necessary
    public record PriceResponse(string Price, string Currency = "$");
}

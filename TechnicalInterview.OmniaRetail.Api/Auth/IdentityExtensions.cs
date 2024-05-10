namespace TechnicalInterview.OmniaRetail.Api.Auth
{
    public static class IdentityExtensions
    {
        public static Guid? GetRetailerId(this HttpContext context)
        {
            System.Security.Claims.Claim? retailerId = context.User.Claims.SingleOrDefault(c => c.Type == "retailerId");

            if (Guid.TryParse(retailerId?.Value, out Guid parsedId))
            {
                return parsedId;
            }

            return null;
        }
    }
}

namespace TechnicalInterview.OmniaRetail.Api.Auth
{
    public static class IdentityExtensions
    {
        public static Guid? GetRetailerId(this HttpContext context)
        {
            return Guid.NewGuid();
            //var retailerId = context.User.Claims.SingleOrDefault(x => x.Type == "retailerId");

            //if (Guid.TryParse(retailerId?.Value, out var parsedId))
            //{
            //    return parsedId;
            //}

            //return null;
        }
    }
}

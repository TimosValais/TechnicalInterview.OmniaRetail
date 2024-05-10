namespace TechnicalInterview.OmniaRetail.Api.Auth
{
    public static class IdentityExtensions
    {
        public static Guid? GetRetailerId(this HttpContext context)
        {
            return new Guid("e3a87e20-36d3-42ee-8993-c8dfbfa01c3b");
            //var retailerId = context.User.Claims.SingleOrDefault(x => x.Type == "retailerId");

            //if (Guid.TryParse(retailerId?.Value, out var parsedId))
            //{
            //    return parsedId;
            //}

            //return null;
        }
    }
}

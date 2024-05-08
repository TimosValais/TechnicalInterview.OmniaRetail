namespace TechnicalInterview.OmniaRetail.Api.Endpoints.Internal
{
    internal interface IEndpoints
    {
        /// <summary>
        /// Should be called before the Run method of the WebApplication.
        /// Method to define the endpoints on a minimal API, their path, their verbage, their query params etc...
        /// </summary>
        /// <param name="app"></param>
        public static abstract void DefineEndPoints(IEndpointRouteBuilder app);
    }
}

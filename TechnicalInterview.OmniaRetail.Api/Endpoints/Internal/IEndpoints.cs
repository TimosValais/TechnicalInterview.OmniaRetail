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
        /// <summary>
        /// Should be called before the Build method of the WebApplicationBuilder.
        /// Any services that should be injected to each endpoint collection should be added here
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        public static abstract void AddServices(IServiceCollection services, IConfiguration configuration);
    }
}

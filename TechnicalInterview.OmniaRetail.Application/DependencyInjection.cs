using Microsoft.Extensions.DependencyInjection;

namespace TechnicalInterview.OmniaRetail.Application
{
    internal static class DependencyInjection
    {
        /// <summary>
        /// Adds the required implementations of the application.
        /// </summary>
        /// <param name="services">The Application Builder Services collection</param>
        /// <returns></returns>
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {

            return services;
        }


        //TODO: the infrastructure layer is in the application for simplicity since the main focus
        //is the business logic, normally it would be in its own project.
        /// <summary>
        /// Adds the required implementations of the infrastructure.
        /// </summary>
        /// <param name="services">The Application Builder Services collection</param>
        /// <returns></returns>
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {

            return services;
        }
    }
}

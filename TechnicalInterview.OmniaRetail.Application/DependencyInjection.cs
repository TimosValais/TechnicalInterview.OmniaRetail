using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TechnicalInterview.OmniaRetail.Application.Persistence;
using TechnicalInterview.OmniaRetail.Application.Persistence.Repositories;
using TechnicalInterview.OmniaRetail.Application.Services;

namespace TechnicalInterview.OmniaRetail.Application
{
    public static class DependencyInjection
    {
        /// <summary>
        /// Adds the required implementations of the application.
        /// </summary>
        /// <param name="services">The Application Builder Services collection</param>
        /// <returns></returns>
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddTransient<IProductService, ProductService>();
            services.AddTransient<IRetailerService, RetailerService>();
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
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<IRetailerProductPriceRepository, RetailerProductPriceRepository>();
            services.AddScoped<IProductGroupRepository, ProductGroupRepository>();
            return services;
        }

        //TODO: the database layer is in the application for simplicity since the main focus
        //is the business logic, normally it would be in its own project.
        /// <summary>
        /// Adds the database.
        /// </summary>
        /// <param name="services">The Application Builder Services collection</param>
        /// <returns></returns>
        public static IServiceCollection AddDatabase(this IServiceCollection services, string connectionString)
        {
            services.AddDbContext<AppDbContext>(options =>
            {
                options.UseSqlite(connectionString);
            });
            services.AddScoped<IDbInitializer, DbInitializer>();
            return services;
        }
    }
}

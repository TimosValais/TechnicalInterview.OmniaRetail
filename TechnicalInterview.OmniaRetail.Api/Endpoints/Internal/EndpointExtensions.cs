using System.Reflection;

namespace TechnicalInterview.OmniaRetail.Api.Endpoints.Internal
{
    internal static class EndpointExtensions
    {
        /// <summary>
        /// Executes te AddEndpoints method of all classes that are not abstract or 
        /// interfaces in the assembly of the marker type.
        /// </summary>
        /// <typeparam name="TMarker">Any Type that belongs to the desired assembly</typeparam>
        /// <param name="services">The services collection of the Web Application Builder</param>
        /// <param name="configuration">The configuration of the Web Application Builder</param>
        public static void AddEndpoints<TMarker>(this IServiceCollection services, IConfiguration configuration)
        {
            AddEndpoints(services, typeof(TMarker), configuration);
        }
        public static void AddEndpoints(this IServiceCollection services, Type typeMarker, IConfiguration configuration)
        {
            IEnumerable<TypeInfo> endpointTypes = GetEndpointTypesFromAssemblyContaining(typeMarker);

            foreach (System.Reflection.TypeInfo? endpointType in endpointTypes)
            {
                endpointType.GetMethod(nameof(IEndpoints.AddServices))!
                    .Invoke(null, new object[] { services, configuration });
            }
        }

        /// <summary>
        /// Defines the endpoints to the Web Application defined by the assembly
        /// of the provided Type Marker.
        /// </summary>
        /// <typeparam name="TMarker">Any Type that belongs to the desired assembly</typeparam>
        /// <param name="app">The Web Application builder (implements IEndpointRouteBuilder) to define the endpoints of the app</param>
        public static void UseEndpoints<TMarker>(this IEndpointRouteBuilder app)
        {
            UseEndpoints(app, typeof(TMarker));
        }
        public static void UseEndpoints(this IEndpointRouteBuilder app, Type typeMarker)
        {
            IEnumerable<TypeInfo> endpointTypes = GetEndpointTypesFromAssemblyContaining(typeMarker);

            foreach (System.Reflection.TypeInfo? endpointType in endpointTypes)
            {
                endpointType.GetMethod(nameof(IEndpoints.DefineEndPoints))!
                    .Invoke(null, new object[] { app });
            }
        }

        private static IEnumerable<TypeInfo> GetEndpointTypesFromAssemblyContaining(Type typeMarker)
        {
            //We only want to get the implemented endpoints and avoid getting the interfaces and abstract classes
            //since their methods wouldn't be invokable.
            return typeMarker.Assembly.DefinedTypes
                .Where(x => !x.IsAbstract && !x.IsInterface &&
                            typeof(IEndpoints).IsAssignableFrom(x));
        }


    }
}

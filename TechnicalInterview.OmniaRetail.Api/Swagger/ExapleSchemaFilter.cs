using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using TechincalInterview.OmniaRetail.Contracts.Requests;

namespace TechnicalInterview.OmniaRetail.Api.Swagger
{
    public class ExampleSchemaFilter : ISchemaFilter
    {
        public void Apply(OpenApiSchema schema, SchemaFilterContext context)
        {
            if (context.Type == typeof(UpdateProductPriceRequest))
            {
                schema.Example = new OpenApiObject()
                {
                    ["price"] = new OpenApiFloat(20.60f),
                    ["productId"] = new OpenApiString("fb97d725-1ca8-4856-a85c-84459d9d31dc"),
                };
            }
            if (context.Type == typeof(TokenGenerationRequest))
            {
                //TODO: need to see how we can define a dictionary here Dict<string,object> for custom claims
                schema.Example = new OpenApiObject()
                {
                    ["retailerId"] = new OpenApiString("bc024a7e-c4f6-42a5-a67d-76b8c9efd432"),
                };
            }
        }
    }
}

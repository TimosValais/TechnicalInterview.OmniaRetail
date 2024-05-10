using TechincalInterview.OmniaRetail.Contracts.Adapters;
using TechnicalInterview.OmniaRetail.Api;
using TechnicalInterview.OmniaRetail.Api.Endpoints.Internal;
using TechnicalInterview.OmniaRetail.Api.Logging;
using TechnicalInterview.OmniaRetail.Application;
using TechnicalInterview.OmniaRetail.Application.Persistence;
WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
ConfigurationManager config = builder.Configuration;

builder.Services.AddTransient(typeof(ILoggerAdapter<>), typeof(LoggerAdapter<>));
builder.Services.AddOutputCache();
builder.Services.AddApplication()
                .AddDatabase(config["Database:ConnectionString"]!)
                .AddInfrastructure();


// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
//Add Application services


WebApplication app = builder.Build();

// Configure the HTTP request pipeline.
//Using Swagger for documentation purposes (Minimal API's help this become even easier)
app.UseSwagger();
app.UseSwaggerUI();



app.UseHttpsRedirection();

app.UseOutputCache();
app.UseEndpoints<IApiMarker>();

// Ensure database is created
// Db initializer is scoped (because it has the DBContext passed into it) so we need
// to do this in a scope.
using (IServiceScope scope = app.Services.CreateScope())
{
    IDbInitializer dbInitializer = scope.ServiceProvider.GetRequiredService<IDbInitializer>();
    await dbInitializer.InitializeAsync();
}
app.Run();


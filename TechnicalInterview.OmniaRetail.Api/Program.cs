using TechnicalInterview.OmniaRetail.Api;
using TechnicalInterview.OmniaRetail.Api.Endpoints.Internal;
using TechnicalInterview.OmniaRetail.Application;
using TechnicalInterview.OmniaRetail.Application.Persistence;
WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
ConfigurationManager config = builder.Configuration;

string? test = config["Database:ConnectionString"];
Console.WriteLine(test);

builder.Services.AddApplication().AddInfrastructure(config["Database:ConnectionString"]!);


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

app.UseEndpoints<IApiMarker>();

IDbInitializer dbInitializer = app.Services.GetRequiredService<IDbInitializer>();
await dbInitializer.InitializeAsync();
app.Run();


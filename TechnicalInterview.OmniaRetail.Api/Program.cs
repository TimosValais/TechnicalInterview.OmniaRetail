using TechnicalInterview.OmniaRetail.Api;
using TechnicalInterview.OmniaRetail.Api.Endpoints.Internal;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
//Add Endpoint specific services
builder.Services.AddEndpoints<IApiMarker>(builder.Configuration);


WebApplication app = builder.Build();

// Configure the HTTP request pipeline.
//Using Swagger for documentation purposes (Minimal API's help this become even easier)
app.UseSwagger();
app.UseSwaggerUI();


app.UseHttpsRedirection();

app.UseEndpoints<IApiMarker>();
app.Run();


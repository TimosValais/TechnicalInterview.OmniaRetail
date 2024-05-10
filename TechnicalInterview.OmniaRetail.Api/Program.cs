using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using TechincalInterview.OmniaRetail.Contracts.Adapters;
using TechnicalInterview.OmniaRetail.Api;
using TechnicalInterview.OmniaRetail.Api.Auth;
using TechnicalInterview.OmniaRetail.Api.Endpoints.Internal;
using TechnicalInterview.OmniaRetail.Api.Logging;
using TechnicalInterview.OmniaRetail.Api.Mappings;
using TechnicalInterview.OmniaRetail.Application;
using TechnicalInterview.OmniaRetail.Application.Persistence;
WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
ConfigurationManager config = builder.Configuration;

builder.Services.AddTransient(typeof(ILoggerAdapter<>), typeof(LoggerAdapter<>));
builder.Services.AddOutputCache();
//Add Application services/database and repos
builder.Services.AddApplication()
                .AddDatabase(config["Database:ConnectionString"]!)
                .AddInfrastructure();
builder.Services.AddValidatorsFromAssemblyContaining<IApiMarker>();

//adding mock Identity only in development
//for demo purposes
if (builder.Environment.IsDevelopment())
{
    builder.Services.AddSingleton<IIdentityService, IdentityService>();
}

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(opts =>
{
    opts.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please insert JWT with Bearer into field. Don't forget to add the word Bearer in front of your JWT!",
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey
    });
    opts.AddSecurityRequirement(new OpenApiSecurityRequirement {
    {
        new OpenApiSecurityScheme
        {
            Reference = new OpenApiReference
            {
              Type = ReferenceType.SecurityScheme,
              Id = "Bearer"
            }
        },
        new string[] { }
    }
  });
});

//add authentication/authorization
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(opts =>
{
    opts.TokenValidationParameters = new()
    {
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(config["Jwt:Key"]!)),
        ValidateIssuerSigningKey = true,
        ValidateLifetime = true,
        ValidIssuer = config["Jwt:Issuer"],
        ValidAudience = config["Jwt:Audience"]
    };
});

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy(AuthConstants.RetailerAdminPolicyName,
            p => p.RequireClaim("retailerId"));
});


WebApplication app = builder.Build();

// Configure the HTTP request pipeline.
//Using Swagger for documentation purposes (Minimal API's help this become even easier)
app.UseSwagger();
app.UseSwaggerUI();



app.UseHttpsRedirection();

//error Middleware
app.Use(async (context, next) =>
{
    try
    {
        await next();
    }
    catch (Exception ex)
    {
        context.Response.StatusCode = 500;
        await context.Response.WriteAsync("An unexpected error has occured. We 're sorry for the inconvenience");
    }
});

app.UseAuthentication();
app.UseAuthorization();


app.UseOutputCache();
app.UseMiddleware<ValidationMappingMiddleware>();

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


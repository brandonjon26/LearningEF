using System.Reflection;
using System.Text;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;
using LearningEF.Api.Data;
using LearningEF.Api.Repositories;
using LearningEF.Api.Services;
using LearningEF.Api.Validation;



// Static name for the policy
const string MyAllowFrontendPolicy = "AllowFrontend";

// -------------------------
// 1. BUILDER CONFIGURATION
// -------------------------

// - Sets the default environment (including the "Development" environment from launchSettings.json).
// - Loads appsettings.json, appsettings.{env}.json, environment variables, and User Secrets.
var builder = WebApplication.CreateBuilder(args);

// --------------------------------------------
// 2. REGISTER SERVICES (Dependency Injection)
// --------------------------------------------

// Register the DbContext using configuration
var connectionString = builder.Configuration.GetConnectionString("DatabaseConnection");
builder.Services.AddDbContext<CarContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDbContext<AppLogContext>(options => 
    options.UseSqlServer(connectionString));

// Register Repositories and Services
builder.Services.AddScoped<ICarRepository, CarRepository>();
builder.Services.AddScoped<CarInterface, CarService>();
builder.Services.AddScoped<LogWriterInterface, LogWriterService>();

// Add CORS Service
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowFrontendPolicy,
        builder =>
        {
            // IMPORTANT: In production, replace localhost with 
            // domain of your React app (e.g., WithOrigins("https://my-app.com"))
            // For development, allow the React default port (3000)
            builder.WithOrigins("http://localhost:3000", "http://localhost:60000")
                   .AllowAnyHeader()
                   .AllowAnyMethod()
                   .AllowCredentials(); // Often needed for auth/cookies later
        });
});

// Add Controller Services (and related MVC services)
builder.Services.AddControllers();

// Add the Fluent Validation service registration
builder.Services.AddValidatorsFromAssemblyContaining<CarValidator>();

// Register Swagger
builder.Services.AddSwaggerGen(options =>
{
    // 1. Define the JWT security scheme
    options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
    {
        Description = "Standard Authorization header using the Bearer scheme. Example: \"Bearer {token}\"",
        In = ParameterLocation.Header,
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey
    });

    // 2. Add the operation filter to include the security definition on protected endpoints
    options.OperationFilter<SecurityRequirementsOperationFilter>();
});

// Add Authentication Service
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
// Configure the JWT Bearer scheme parameters
.AddJwtBearer(options =>
{
    // Get the IConfiguration object via the builder
    var configuration = builder.Configuration;

    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,

        // Retrieve values from the "Jwt" section of appsettings.json
        ValidIssuer = configuration["Jwt:Issuer"],
        ValidAudience = configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(configuration["Jwt:SecretKey"] ??
            throw new InvalidOperationException("JWT Secret Key not found in configuration."))
        )
    };
});

// --------------------------------------------
// 3. BUILD AND CONFIGURE APPLICATION PIPELINE
// --------------------------------------------

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    // These two lines enable the Swagger JSON endpoint and the Swagger UI
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Configure the HTTP Request Pipeline (Middleware)
// Order matters for middleware:
if (!app.Environment.IsDevelopment())
{
    app.UseHttpsRedirection();
}
app.UseRouting();

// Activate CORS Here
app.UseCors(MyAllowFrontendPolicy);

app.UseAuthentication();
app.UseAuthorization();

// 4. MAP CONTROLLER ENDPOINTS HERE
app.MapControllers();

// 5. Run the application
await app.RunAsync();
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using LearningEF.Data;         // Assuming CarContext is here
using LearningEF.Repositories;  // Assuming ICarRepository and CarRepository are here
using LearningEF.Services;      // Assuming CarInterface and CarService are here



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

// Register Repositories and Services
builder.Services.AddTransient<ICarRepository, CarRepository>();
builder.Services.AddTransient<CarInterface, CarService>();

// Add Controller Services (and related MVC services)
builder.Services.AddControllers();

// Optional: Add Swagger/OpenAPI support for API testing documentation
// builder.Services.AddEndpointsApiExplorer();
// builder.Services.AddSwaggerGen();

// --------------------------------------------
// 3. BUILD AND CONFIGURE APPLICATION PIPELINE
// --------------------------------------------

var app = builder.Build();

// Configure the HTTP Request Pipeline (Middleware)
// Order matters for middleware:
app.UseHttpsRedirection();
app.UseRouting();

// 4. MAP CONTROLLER ENDPOINTS HERE (Replaces UseEndpoints(endpoints => endpoints.MapControllers()))
app.MapControllers();

// 5. Run the application
await app.RunAsync();
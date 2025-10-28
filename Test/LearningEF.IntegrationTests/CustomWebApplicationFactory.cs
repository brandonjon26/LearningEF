using LearningEF.Api; 
using LearningEF.Api.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using System.Linq;
using System.Reflection;

namespace LearningEF.IntegrationTests
{
    // TEntryPoint is the 'Program' class from the API assembly
    public class CustomWebApplicationFactory<TEntryPoint>
        : WebApplicationFactory<TEntryPoint> where TEntryPoint : class
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                // Find and remove the existing CarContext registration (SQL Server connection)
                var dbContextDescriptor = services.SingleOrDefault(
                    d => d.ServiceType == typeof(DbContextOptions<CarContext>));

                if (dbContextDescriptor != null)
                {
                    services.Remove(dbContextDescriptor);
                }

                // Add CarContext using a unique in-memory database
                services.AddDbContext<CarContext>(options =>
                {
                    // Use a unique name to ensure test isolation
                    options.UseInMemoryDatabase("IntegrationTestDb");
                });

                // Ensure the DbContext uses the scoped services
                var sp = services.BuildServiceProvider();

                using (var scope = sp.CreateScope())
                {
                    var scopedServices = scope.ServiceProvider;
                    var db = scopedServices.GetRequiredService<CarContext>();

                    // Ensure the database is created and clear any old data
                    db.Database.EnsureDeleted();
                    db.Database.EnsureCreated();
                }
            });
        }

        protected override IHost CreateHost(IHostBuilder builder)
        {
            // Set the content root to the API project's output directory.
            builder.UseContentRoot(GetContentRootPath());

            return base.CreateHost(builder);
        }

        // Helper method to reliably find the API project's path
        private static string GetContentRootPath()
        {
            // The test runner executes inside a path like: Solution/Test/IntegrationTests/bin/Debug/net8.0/
            var testAssemblyPath = Assembly.GetExecutingAssembly().Location;

            // Navigate up to the solution root (4 levels up from the DLL)
            var solutionRoot = Path.GetFullPath(Path.Combine(testAssemblyPath, "../../../../../../"));

            // Now, combine with the API project's folder name
            var apiProjectPath = Path.Combine(solutionRoot, "LearningEF.Api");

            if (!Directory.Exists(apiProjectPath))
            {
                throw new InvalidOperationException($"API project root not found at: {apiProjectPath}");
            }

            return apiProjectPath;
        }

        // Override the CreateHost method to explicitly set the content root
        //protected override IHost CreateHost(IHostBuilder builder)
        //{
        //    // The previous implementation was slightly incorrect. 
        //    // We need to apply the content root to the builder BEFORE calling base.CreateHost(builder)

        //    // CRITICAL FIX: Explicitly set the content root path.
        //    // The GetProjectPath helper is designed to locate the API project's directory relative to the solution.
        //    builder.UseContentRoot(GetProjectPath("LearningEF.Api", Assembly.GetExecutingAssembly().GetName().Name));

        //    // Now, call the base implementation to continue building and running the host.
        //    return base.CreateHost(builder); // <--- This line implicitly calls the logic
        //}

        // Helper method to resolve the path
        private static string GetProjectPath(string projectFileName, string startupAssemblyName)
        {
            // Get the full path to the project file (e.g., /path/to/LearningEF.Api.csproj)
            var assembly = Assembly.Load(new AssemblyName(startupAssemblyName));
            var projectDirectory = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..\\..\\..\\..\\"));

            // This loop searches up the directory tree until it finds the project directory
            var solutionDirectory = Path.GetFullPath(Path.Combine(projectDirectory, ".."));

            // Assuming your solution directory structure is standard:
            return Path.Combine(solutionDirectory, projectFileName);
        }
    }
}
using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using LearningEF.Data;
using LearningEF.Repositories;
using LearningEF.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Builder; // Added for WebApplicationBuilder/Host extensions
using Microsoft.AspNetCore.Mvc; // Added for Controller configuration

namespace LearningEF.Models
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            // Build the Web Host and Run
            await CreateWebHostBuilder(args).Build().RunAsync();
        }

        public static IHostBuilder CreateWebHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseEnvironment("Development") // Critical Fix preserved
                .ConfigureAppConfiguration((hostContext, config) =>
                {
                    // 1. Clear default configuration sources
                    config.Sources.Clear();

                    // 2. Load the base appsettings.json file
                    config.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

                    // 3. Load the environment-specific file
                    config.AddJsonFile($"appsettings.{hostContext.HostingEnvironment.EnvironmentName}.json", optional: true, reloadOnChange: true);

                    // 4. Load Environment Variables
                    config.AddEnvironmentVariables();

                    // 5. Load Secret Manager (Guaranteed to load due to UseEnvironment("Development"))
                    config.AddUserSecrets<Program>();
                })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    // This is where web-specific services and middleware are configured.
                    webBuilder.ConfigureServices((hostContext, services) =>
                    {
                        IConfiguration configuration = hostContext.Configuration;
                        string connectionString = configuration.GetConnectionString("DatabaseConnection");

                        // 1. Register the DbContext
                        services.AddDbContext<CarContext>(options =>
                            options.UseSqlServer(connectionString));

                        // 2. Register Repositories and Services
                        services.AddTransient<ICarRepository, CarRepository>();
                        services.AddTransient<CarInterface, CarService>();

                        // 3. ADD CONTROLLER SERVICES HERE
                        services.AddControllers();
                    });

                    webBuilder.Configure(app =>
                    {
                        // Standard middleware for routing and authorization/authentication
                        app.UseRouting();
                        // app.UseAuthorization(); // Add this line if needed later

                        // 4. MAP CONTROLLER ENDPOINTS HERE
                        app.UseEndpoints(endpoints =>
                        {
                            endpoints.MapControllers();
                        });
                    });
                });
    }
}
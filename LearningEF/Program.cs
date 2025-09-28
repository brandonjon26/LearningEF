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

namespace LearningEF.Models
{ 
    public class Program
    {
        [STAThread]
        public static async Task Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();

            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                try
                {
                    bool processSuccess = false;

                    // Get your service and run the main application logic
                    CarInterface carService = services.GetRequiredService<CarInterface>();

                    // Assuming your service has a method like 'ProcessCarCreation' 
                    // that handles the console input and repository call.
                    processSuccess = await carService.CreateCar(args);

                    if (processSuccess)
                    {
                        Console.WriteLine("App was successful!");
                    }
                    else
                    {
                        Console.WriteLine("Try Again.");
                    }
                }
                catch (Exception ex)
                {
                    // Log the error (optional, but good practice)
                    Console.WriteLine($"An error occurred: {ex.Message}");
                }
            }

            Console.ReadLine();
            Environment.Exit(0);
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .UseEnvironment("Development")
            .ConfigureAppConfiguration((hostContext, config) =>
            {
                // 1. Clear any default configuration settings that might be confusing the system
                config.Sources.Clear();

                // 2. Load the base appsettings.json file
                config.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

                // 3. Load the environment-specific file (appsettings.development.json)
                // hostContext.HostingEnvironment.EnvironmentName will now ALWAYS be "Development"
                config.AddJsonFile($"appsettings.{hostContext.HostingEnvironment.EnvironmentName}.json", optional: true, reloadOnChange: true);

                // 4. Load Environment Variables (optional but good practice)
                config.AddEnvironmentVariables();

                string currentEnvironment = hostContext.HostingEnvironment.EnvironmentName;
                Console.WriteLine($"*** ENVIRONMENT NAME: {currentEnvironment} ***");

                // 5. Load Secret Manager (Will now ALWAYS load since the environment is forced to Development)
                config.AddUserSecrets<Program>();
            })
            .ConfigureServices((hostContext, services) =>
            {
                IConfiguration configuration = hostContext.Configuration;
                string connectionString = configuration.GetConnectionString("DatabaseConnection");

                Console.WriteLine($"Connection string: {connectionString}");

                // 1. Register the DbContext
                services.AddDbContext<CarContext>(options =>
                    options.UseSqlServer(connectionString));

                // 2. Register Repositories and Services
                // The interface ICarRepository will map to the concrete CarRepository implementation
                services.AddTransient<ICarRepository, CarRepository>();

                // The interface CarInterface will map to the concrete CarService implementation
                services.AddTransient<CarInterface, CarService>();
            });
    }        
}
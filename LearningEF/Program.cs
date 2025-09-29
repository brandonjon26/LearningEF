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

//namespace LearningEF.Models
//{ 
//    public class Program
//    {
//        [STAThread]
//        public static async Task Main(string[] args)
//        {
//            var host = CreateHostBuilder(args).Build();

//            using (var scope = host.Services.CreateScope())
//            {
//                var services = scope.ServiceProvider;
//                try
//                {
//                    bool processSuccess = false;

//                    // Get your service and run the main application logic
//                    CarInterface carService = services.GetRequiredService<CarInterface>();

//                    // Choose how the app will run
//                    Console.WriteLine("Select mode:\n1 = Add Record\n2 = Get All Records");
//                    int mode = Convert.ToInt16(Console.ReadLine());

//                    switch (mode)
//                    {
//                        case 1:
//                            // Add a car
//                            processSuccess = await carService.CreateCarAsync();
//                            break;

//                        case 2:
//                            // Get all cars
//                            List<Car> cars = new List<Car>();
//                            (processSuccess, cars) = await carService.ListAllCarsAsync();

//                            if (cars.Count == 0)
//                            {
//                                Console.WriteLine("No cars found.");
//                            }
//                            else
//                            {
//                                Console.WriteLine("Here are all of the cars:\n");
//                                foreach (Car car in cars)
//                                {
//                                    Console.WriteLine($"{car.Color} {car.Make} {car.Model}");
//                                }

//                                Console.ReadLine();
//                            }
//                            break;
//                    }                    

//                    if (processSuccess)
//                    {
//                        Console.WriteLine("App was successful!");
//                    }
//                    else
//                    {
//                        Console.WriteLine("Try Again.");
//                    }
//                }
//                catch (Exception ex)
//                {
//                    // Log the error
//                    Console.WriteLine($"An error occurred: {ex.Message}");
//                }
//            }

//            Console.ReadLine();
//            Environment.Exit(0);
//        }

//        public static IHostBuilder CreateHostBuilder(string[] args) =>
//        Host.CreateDefaultBuilder(args)
//            .UseEnvironment("Development")
//            .ConfigureAppConfiguration((hostContext, config) =>
//            {
//                // 1. Clear any default configuration settings that might be confusing the system
//                config.Sources.Clear();

//                // 2. Load the base appsettings.json file
//                config.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

//                // 3. Load the environment-specific file (appsettings.development.json)
//                // hostContext.HostingEnvironment.EnvironmentName will now ALWAYS be "Development"
//                config.AddJsonFile($"appsettings.{hostContext.HostingEnvironment.EnvironmentName}.json", optional: true, reloadOnChange: true);

//                // 4. Load Environment Variables
//                config.AddEnvironmentVariables();

//                string currentEnvironment = hostContext.HostingEnvironment.EnvironmentName;
//                Console.WriteLine($"*** ENVIRONMENT NAME: {currentEnvironment} ***");

//                // 5. Load Secret Manager (Will now ALWAYS load since the environment is forced to Development)
//                config.AddUserSecrets<Program>();
//            })
//            .ConfigureServices((hostContext, services) =>
//            {
//                IConfiguration configuration = hostContext.Configuration;
//                string connectionString = configuration.GetConnectionString("DatabaseConnection");

//                Console.WriteLine($"Connection string: {connectionString}");

//                // 1. Register the DbContext
//                services.AddDbContext<CarContext>(options =>
//                    options.UseSqlServer(connectionString));

//                // 2. Register Repositories and Services
//                // The interface ICarRepository will map to the concrete CarRepository implementation
//                services.AddTransient<ICarRepository, CarRepository>();

//                // The interface CarInterface will map to the concrete CarService implementation
//                services.AddTransient<CarInterface, CarService>();
//            });
//    }        
//}
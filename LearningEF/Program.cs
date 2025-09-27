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
            .ConfigureServices((hostContext, services) =>
            {
                IConfiguration configuration = hostContext.Configuration;
                string connectionString = configuration.GetConnectionString("DatabaseConnection");

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
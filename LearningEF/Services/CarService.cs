using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using LearningEF.Models;
using LearningEF.Repositories;

namespace LearningEF.Services
{
    public class CarService : CarInterface
    {
        private readonly ICarRepository _carRepository;

        public CarService(ICarRepository carRepository)
        {
            _carRepository = carRepository;
        }

        public async Task<bool> CreateCarAsync(Car newCar)
        {
            await Task.Delay(0);
            Car car = new Car();

            Console.WriteLine("Lets add a new Car!");
            try
            {
                // Get the make
                Console.WriteLine("Please Enter the make of this car:");
                car.Make = Console.ReadLine();

                // Get the model
                Console.WriteLine("Please enter the model of this car:");
                car.Model = Console.ReadLine();

                // Get the Year
                int year;
                bool isValidYear = false;
                do
                {
                    Console.WriteLine("Please enter the year of this car:");
                    string yearInput = Console.ReadLine();

                    // Make sure input is valid
                    if (int.TryParse(yearInput, out year))
                    {
                        isValidYear = true;
                        car.Year = year;
                    }
                    else
                    {
                        Console.WriteLine("Invalid year, please enter a whole number.");
                    }
                }
                while (!isValidYear);

                // Get the Color
                Console.WriteLine("Enter the color of the car:");
                car.Color = Console.ReadLine();

                car.DateCreated = DateTime.Now;
                car.DateModified = DateTime.Now;

                _carRepository.AddCar(car);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }            
        }

        public async Task<(bool, List<Car>)> ListAllCarsAsync()
        {
            List<Car> cars = new List<Car>();

            try
            {
                cars = await _carRepository.GetListAsync();
                return (true, cars);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.ReadLine();
                return (false, cars);
            }
        }
    }
}

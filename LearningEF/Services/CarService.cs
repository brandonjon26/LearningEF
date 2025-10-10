using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
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
            try
            {
                newCar.DateCreated = DateTime.Now;
                newCar.DateModified = DateTime.Now;

                // Stage the car for insertion
                _carRepository.AddCar(newCar);

                // Commit the changes to the database
                int rowsAffected = await _carRepository.SaveChangesAsync();

                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                // Log the error (placeholder for proper logging; maybe use ILogger or write my own logging)
                Debug.WriteLine($"Error creating car: {ex.Message}");

                return false;
            }            
        }

        public async Task<(bool, List<Car>)> ListAllCarsAsync()
        {
            List<Car> cars = new List<Car>();

            try
            {
                // Get the list of cars from the database
                cars = await _carRepository.GetListAsync();
                return (true, cars);
            }
            catch (Exception ex)
            {
                // Log the error (placeholder for proper logging; maybe use ILogger or write my own logging)
                Debug.WriteLine($"Error getting cars: {ex.Message}");
                return (false, cars);
            }
        }
    }
}

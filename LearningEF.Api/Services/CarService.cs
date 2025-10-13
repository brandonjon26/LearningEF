using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using Microsoft.EntityFrameworkCore;
using LearningEF.Api.Models;
using LearningEF.Api.Repositories;

namespace LearningEF.Api.Services
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

        public async Task<Car?> GetCarByIdAsync(long carId)
        {
            // Go get the car
            return await _carRepository.GetByIdAsync(carId);
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

        public async Task<bool> RemoveCarAsync(long carId)
        {
            try
            {
                Car? car = await _carRepository.GetByIdAsync(carId);

                if (car != null)
                {
                    // Remove the car from the database
                    _carRepository.DeleteCar(car);
                    int rowsAffected = await _carRepository.SaveChangesAsync();

                    return rowsAffected > 0;
                }

                return false;
            }
            catch (Exception ex)
            {
                // Log the error (placeholder for proper logging; maybe use ILogger or write my own logging)
                Debug.WriteLine($"Error removing car: {ex.Message}");
                return false;
            }
        }

        public async Task<(bool, Car?)> ChangeCarAsync(long carId, Car updatedCar)
        {
            // Get the existing car from the database
            Car? existingCar = await _carRepository.GetByIdAsync(carId);

            try
            {
                if (existingCar != null)
                {
                    // Map new properties
                    existingCar.Make = updatedCar.Make;
                    existingCar.Model = updatedCar.Model;
                    existingCar.Year = updatedCar.Year;
                    existingCar.Color = updatedCar.Color;
                    existingCar.Price = updatedCar.Price;
                    existingCar.DateModified = DateTime.Now;

                    // Commit the change
                    int rowsAffected = await _carRepository.SaveChangesAsync();

                    return (rowsAffected > 0, existingCar);
                }

                // Car was not found
                return (false, null);
            }
            catch (Exception ex)
            {
                // Log the error (placeholder for proper logging; maybe use ILogger or write my own logging)
                Debug.WriteLine($"Error removing car: {ex.Message}");
                return (false, null);
            }
        } 
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using LearningEF.Models;
using LearningEF.Data;

namespace LearningEF.Repositories
{
    public class CarRepository : ICarRepository
    {
        private readonly CarContext _context;
        public CarRepository(CarContext context)
        {
            _context = context;
        }

        public async Task<int> SaveChangesAsync()
        {
            // Write changes to the database
            return await _context.SaveChangesAsync();
        }

        public void AddCar(Car car)
        {
            // Stage changes to the database
            _context.Cars.Add(car);            
        }        

        public async Task<List<Car>> GetListAsync()
        {            
            // Get all cars from Car table
            return await _context.Cars.OrderBy(car => car.CarId).ToListAsync();
        }

        public async Task<Car> GetByIdAsync(long carId)
        {
            // Get the specific car
            return await _context.Cars.FindAsync(carId);
        }

        public void DeleteCar(Car car)
        {
            // Stage the car for deletion
            _context.Cars.Remove(car);
        }
    }
}

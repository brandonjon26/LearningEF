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
        public void AddCar(Car car)
        {
            // Write to the database
            _context.Cars.Add(car);
            int rowsAffected = _context.SaveChanges();
            Console.WriteLine($"Successfully saved car to DB, Rows affected: {rowsAffected}");
        }
    }
}

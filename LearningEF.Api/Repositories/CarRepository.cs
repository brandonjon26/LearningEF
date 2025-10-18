using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using LearningEF.Api.Models;
using LearningEF.Api.Data;

namespace LearningEF.Api.Repositories
{
    public class CarRepository : BaseRepository<Car, long>, ICarRepository
    {       
        public CarRepository(CarContext context) : base(context)
        {            
        }

        public void AddCar(Car car)
        {
            // Stage changes to the database
            DbSet.Add(car);            
        }        

        public async Task<List<Car>> GetListAsync()
        {            
            // Get all cars from Car table
            return await DbSet.OrderBy(car => car.CarId).ToListAsync();
        }

        public void DeleteCar(Car car)
        {
            // Stage the car for deletion
            DbSet.Remove(car);
        }
    }
}

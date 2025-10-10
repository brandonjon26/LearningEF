using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LearningEF.Models;

namespace LearningEF.Repositories
{
    public interface ICarRepository : IBaseRepository<Car, long>
    {
        void AddCar(Car car);        
        Task<List<Car>> GetListAsync();        
        void DeleteCar(Car car);
    }
}

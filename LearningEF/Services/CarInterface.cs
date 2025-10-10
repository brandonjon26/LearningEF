using LearningEF.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearningEF.Services
{
    public interface CarInterface
    {
        Task<bool> CreateCarAsync(Car newCar);
        Task<Car?> GetCarByIdAsync(long id);
        Task<(bool, List<Car>)> ListAllCarsAsync();
        Task<bool> RemoveCarAsync(long id);
        Task<(bool, Car?)> ChangeCarAsync(long id, Car car);
    }
}

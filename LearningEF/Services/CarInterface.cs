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
        Task<(bool, List<Car>)> ListAllCarsAsync();
    }
}

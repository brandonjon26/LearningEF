using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LearningEF.Models;

namespace LearningEF.Repositories
{
    public interface ICarRepository
    {
        void AddCar(Car car);
    }
}

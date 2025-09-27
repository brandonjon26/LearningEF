using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearningEF.Services
{
    internal interface CarInterface
    {
        Task <bool> CreateCar(string[] args);
    }
}

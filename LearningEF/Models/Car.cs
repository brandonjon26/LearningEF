using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace LearningEF.Models
{
    public class Car
    {
        public long CarId { get; set; } = 0;
        public string Make { get; set; } = "";
        public string Model { get; set; } = "";
        public int Year { get; set; } = 0;
        public string Color { get; set; } = "";
        public DateTime DateCreated { get; set; } = new DateTime(1900, 01, 01);
        public DateTime DateModified { get; set; } = new DateTime(1900, 01, 01);
    }
}

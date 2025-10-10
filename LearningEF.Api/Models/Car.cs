using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearningEF.Api.Models
{
    public class Car
    {
        public long CarId { get; set; } = 0;
        public string? Make { get; set; } = "";
        public string? Model { get; set; } = "";
        [Required]
        [Range(1900, 2026, ErrorMessage = "Year must be between 1900 and the current year plus one.")]
        public int Year { get; set; }
        public string? Color { get; set; } = "";
        public DateTime DateCreated { get; set; } = new DateTime(1900, 01, 01);
        public DateTime DateModified { get; set; } = new DateTime(1900, 01, 01);
    }
}

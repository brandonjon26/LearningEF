using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LearningEF.Api.Models;

namespace LearningEF.Api.Data
{
    public class CarContext : DbContext
    {
        public DbSet<Car> Cars { get; set; }

        public CarContext(DbContextOptions<CarContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Use the Fluent API to explicitly map the Car model to the 'Car' table name
            modelBuilder.Entity<Car>().ToTable("Car");

            // ALWAYS call the base implementation last
            base.OnModelCreating(modelBuilder);
        }
    }
}

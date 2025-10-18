using Xunit;
using Moq;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.InMemory;
using Microsoft.EntityFrameworkCore.Query;
using LearningEF.Api.Data;
using LearningEF.Api.Models;
using LearningEF.Api.Repositories;
using LearningEF.UnitTests.TestHelpers;


namespace LearningEF.UnitTests
{
    public class CarRepositoryTests
    {
        // Mock the CarContext
        private readonly Mock<CarContext> _mockContext;
        private readonly CarRepository _repository;

        public CarRepositoryTests()
        {
            // ARRANGE
            _mockContext = new Mock<CarContext>();
            _repository = new CarRepository(_mockContext.Object); // Instantiate the repository using the mocked context
        }

        // Helper method to create a mock DbSet from a list of entities
        private Mock<DbSet<T>> GetMockDbSet<T>(IQueryable<T> entities) where T : class
        {
            Mock<DbSet<T>> mockSet = new Mock<DbSet<T>>();

            mockSet.As<IQueryable<T>>().Setup(m => m.Provider).Returns(new TestAsyncQueryProvider<T>(entities.Provider));
            mockSet.As<IQueryable<T>>().Setup(m => m.Expression).Returns(entities.Expression);
            mockSet.As<IQueryable<T>>().Setup(m => m.ElementType).Returns(entities.ElementType);
            mockSet.As<IQueryable<T>>().Setup(m => m.GetEnumerator()).Returns(entities.GetEnumerator());

            // Setup the ASYNCHRONOUS parts
            mockSet.As<IAsyncEnumerable<T>>()
                   .Setup(m => m.GetAsyncEnumerator(It.IsAny<CancellationToken>()))
                   .Returns(new TestAsyncEnumerator<T>(entities.GetEnumerator()));

            // Mock FindAsync (for GetCarById)
            // NOTE: This FindAsync mock is often simplified, but depends on BaseRepository's Find method.
            // Assuming FindAsync(id):
            mockSet.Setup(m => m.FindAsync(It.IsAny<object[]>()))
                   .Returns<object[]>(ids =>
                   {
                       // Since this is ONLY called from the CarRepositoryTests file, 
                       // we can safely cast and look up by CarId (assuming CarId is the key).
                       var id = Convert.ToInt32(ids[0]);
                       var foundEntity = entities.OfType<Car>().FirstOrDefault(e => e.CarId == id);

                       // The return type MUST be Task<T> or ValueTask<T>
                       return ValueTask.FromResult(foundEntity as T);
                   });

            return mockSet;
        }

        [Fact]
        public async Task GetCars_ShouldReturnAllCars()
        {
            // ARRANGE (Setup the Mock)
            IQueryable<Car> expectedCars = new List<Car>
            {
                new Car { CarId = 1, Make = "Honda", Model = "Civic", Year = 2020, Price = 25000.00m },
                new Car { CarId = 2, Make = "Toyota", Model = "Corolla", Year = 2022, Price = 30000.50m }
            }.AsQueryable();

            Mock<DbSet<Car>> mockDbSet = GetMockDbSet(expectedCars);

            // Tell the mock context to return our mock DbSet when 'Cars' is accessed
            _mockContext.Setup(c => c.Cars).Returns(mockDbSet.Object); // Property access mock
            _mockContext.Setup(c => c.Set<Car>()).Returns(mockDbSet.Object); // Generic method access mock

            // Call the method being tested
            List<Car> result = await _repository.GetListAsync();

            // ASSERT (Verify the results)
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
            Assert.Equal("Civic", result.First().Model);
        }

        [Fact]
        public async Task GetCarById_ShouldReturnCorrectCar()
        {
            // ARRANGE
            var testCar = new Car { CarId = 5, Make = "Ford", Model = "Focus", Year = 2018, Price = 15000.50m };
            var carsList = new List<Car> { testCar }.AsQueryable();
            var mockDbSet = GetMockDbSet(carsList);
            _mockContext.Setup(c => c.Cars).Returns(mockDbSet.Object); // Property access mock
            _mockContext.Setup(c => c.Set<Car>()).Returns(mockDbSet.Object); // Generic method access mock

            // ACT
            var result = await _repository.GetByIdAsync(5);

            // ASSERT
            Assert.NotNull(result);
            Assert.Equal(5, result.CarId);
            Assert.Equal("Focus", result.Model);
        }
    }
}


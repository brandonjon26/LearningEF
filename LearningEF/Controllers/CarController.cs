using LearningEF.Models; // Ensure this references your Car model
using LearningEF.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

// Controller for all API operations related to the Car resource
[Route("api/[controller]")] // Sets the base route to /api/car
[ApiController]
public class CarController : ControllerBase
{
    private readonly CarInterface _carService;

    // Constructor: Dependency Injection automatically provides the CarService
    public CarController(CarInterface carService)
    {
        _carService = carService;
    }

    // GET api/car
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Car>>> GetAllCars()
    {
        // 1. Call the service layer to get the data
        var cars = await _carService.ListAllCarsAsync();

        // 2. Handle the response
        if (cars.Item1 == false || !cars.Item2.Any())
        {
            // Returns HTTP 204 No Content if the list is empty (A good REST practice)
            return NoContent();
        }

        // Returns HTTP 200 OK with the list of cars serialized as JSON
        return Ok(cars);
    }

    // POST api/car
    [HttpPost]
    public async Task<ActionResult<Car>> CreateCar([FromBody] Car newCar)
    {
        // The [FromBody] attribute tells the framework to deserialize the JSON request body into a Car object.

        // 1. Call the service layer to create the record
        // NOTE: You may need to update your CarService.CreateCarAsync to accept a Car object instead of parsing console input.
        bool success = await _carService.CreateCarAsync(newCar);

        if (success)
        {
            // Returns HTTP 201 Created
            return CreatedAtAction(nameof(GetAllCars), newCar);
        }
        else
        {
            // Returns HTTP 400 Bad Request
            return BadRequest("Failed to create car record.");
        }
    }
}
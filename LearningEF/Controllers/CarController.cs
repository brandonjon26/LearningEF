using LearningEF.Models; // Ensure this references your Car model
using LearningEF.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using static System.Net.WebRequestMethods;

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
        // Get the data
        var cars = await _carService.ListAllCarsAsync();

        // Handle the response
        if (cars.Item1 == false || !cars.Item2.Any())
        {
            // Returns HTTP 204 No Content if the list is empty (A good REST practice)
            return NoContent();
        }

        // Returns HTTP 200 OK with the list of cars serialized as JSON
        return Ok(cars.Item2);
    }

    // POST api/car
    [HttpPost]
    public async Task<ActionResult<Car>> CreateCar([FromBody] Car newCar) // The [FromBody] attribute tells the framework to deserialize the JSON request body into a Car object.
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }        

        // Only if ModelState is valid do we try to write this record
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

    // DELETE api/car/{id}
    [HttpDelete("{id}")]
    public async Task<ActionResult<Car>> DeleteCar(int id)
    {
        bool success = await _carService.RemoveCarAsync(id);

        if (success)
        {
            // Return 204 No Content for a successful deletion
            return NoContent();
        }
        else
        {
            // HTTP 404: The car was not found or deletion failed for another reason
            return NotFound($"Car with ID {id} not found.");
        }              
    }
}
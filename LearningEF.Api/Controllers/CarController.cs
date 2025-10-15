using LearningEF.Api.Data;
using LearningEF.Api.Models;
using LearningEF.Api.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Data;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using static System.Net.WebRequestMethods;

// Controller for all API operations related to the Car resource
[Route("api/[controller]")] // Sets the base route to /api/car
[ApiController]
public class CarController : ControllerBase
{    
    private readonly CarInterface _carService;
    private readonly LogWriterInterface _logWriterService;

    // Constructor: Dependency Injection automatically provides the CarService
    public CarController(LogWriterInterface logWriterService, CarInterface carService)
    {
        _carService = carService;
        _logWriterService = logWriterService;
    }

    // GET api/car
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Car>>> GetAllCars()
    {
        try
        {
            // Get the data
            var (success, data) = await _carService.ListAllCarsAsync();

            // Handle the response
            if (!success || !data.Any())
            {
                // Log empty table
                await _logWriterService.LogInformationAsync("No cars were found in the database", nameof(CarController));

                // Returns HTTP 204 No Content if the list is empty (A good REST practice)
                return NoContent();
            }

            // Returns HTTP 200 OK with the list of cars serialized as JSON
            return Ok(data);
        }
        catch (Exception ex)
        {
            // Log the critical error
            await _logWriterService.LogErrorAsync(ex, "FATAL ERROR trying to get cars.", nameof(CarController));

            // Return a 500 status code to the client
            return StatusCode(500, "Internal server error: A critical error occurred trying to get cars.");
        }        
    }

    // Get api/Car/{id}
    [HttpGet("{id}")]
    public async Task<ActionResult<Car>> GetCarAsync(long id)
    {
        try
        {
            Car? car = await _carService.GetCarByIdAsync(id);

            if (car == null)
            {
                // Log car not found
                await _logWriterService.WriteLogAsync(
                    LearningEF.Api.Models.LogLevel.Warning, 
                    $"{nameof(CarController)} The specific car was not found - carId {id}",
                    null,
                    new { CarIdAttempted = id }
                );

                // HTTP 404: The car was not found
                return NotFound($"Car with ID {id} not found.");
            }

            // HTTP 200: OK (Car object was found)
            return Ok(car);
        }
        catch (Exception ex)
        {
            // Log the critical error
            await _logWriterService.LogErrorAsync(ex, $"FATAL ERROR trying to get car Id {id}.", nameof(CarController));

            // Return a 500 status code to the client
            return StatusCode(500, "Internal server error: A critical error occurred trying to get the specific car.");
        }        
    }

    // POST api/car
    [HttpPost]
    public async Task<ActionResult<Car>> CreateCar([FromBody] Car newCar) // The [FromBody] attribute tells the framework to deserialize the JSON request body into a Car object.
    {
        if (!ModelState.IsValid)
        {
            // Log a warning for bad client data
            await _logWriterService.LogInformationAsync($"CreateCar failed due to invalid model state for car {newCar.Make} {newCar.Model}.", nameof(CarController));
            return BadRequest(ModelState);
        }

        try
        {
            // Only if ModelState is valid do we try to write this record
            bool success = await _carService.CreateCarAsync(newCar);

            if (success)
            {
                // Log successful creation
                await _logWriterService.WriteLogAsync(
                    LearningEF.Api.Models.LogLevel.Information, 
                    $"{nameof(CarController)} Car created successfully: {newCar.CarId}", 
                    null, 
                    new { CarId = newCar.CarId, Make = newCar.Make, Model = newCar.Model }
                );

                // Returns HTTP 201 Created
                return CreatedAtAction(nameof(GetAllCars), newCar);
            }
            else
            {
                // Log a warning if the service layer explicitly returned false
                await _logWriterService.LogErrorAsync(new Exception("Service layer returned false"), $"Car creation failed in service layer for car: {newCar.Make} {newCar.Model}", nameof(CarController));

                // Returns HTTP 400 Bad Request
                return BadRequest("Failed to create car record.");
            }
        }
        catch (Exception ex)
        {
            // Log the critical error
            await _logWriterService.LogErrorAsync(ex, $"FATAL ERROR during car creation for car: {newCar.Make} {newCar.Model}", nameof(CarController));

            // Return a 500 status code to the client
            return StatusCode(500, "Internal server error: A critical error occurred during car creation.");
        }        
    }

    // DELETE api/car/{id}
    [HttpDelete("{id}")]
    public async Task<ActionResult<Car>> DeleteCar(long id)
    {
        try
        {
            bool success = await _carService.RemoveCarAsync(id);

            if (success)
            {
                // Return 204 No Content for a successful deletion
                return NoContent();
            }
            else
            {
                // Log car not found
                await _logWriterService.WriteLogAsync(
                    LearningEF.Api.Models.LogLevel.Warning, 
                    $"{nameof(CarController)} The specific car was not found - carId {id}",
                    null,
                    new { CarIdAttempted = id }
                );

                // HTTP 404: The car was not found or deletion failed for another reason
                return NotFound($"Car with ID {id} not found.");
            }
        }
        catch (Exception ex)
        {
            // Log the critical error
            await _logWriterService.LogErrorAsync(ex, $"FATAL ERROR during car deletion for car Id {id}", nameof(CarController));

            // Return a 500 status code to the client
            return StatusCode(500, "Internal server error: A critical error occurred during car deletion.");
        }        
    }

    // UPDATE api/car/{id}
    [HttpPut("{id}")]
    public async Task<ActionResult<Car>> UpdateCarAsync(long id, [FromBody] Car car)
    {
        try
        {
            var (success, updatedCar) = await _carService.ChangeCarAsync(id, car);

            if (success)
            {
                // Return 200 OK for a successful update
                return Ok(updatedCar);
            }
            else
            {
                // Log car not found
                await _logWriterService.WriteLogAsync(
                    LearningEF.Api.Models.LogLevel.Warning, 
                    $"{nameof(CarController)} The specific car was not found - carId {id}",
                    null,
                    new { CarIdAttempted = id }
                );

                // HTTP 404: The car was not found or update failed
                return NotFound($"Car with ID {id} not found.");
            }
        }
        catch (Exception ex)
        {
            // Log the critical error
            await _logWriterService.LogErrorAsync(ex, $"FATAL ERROR during car update for car: {car.Make} {car.Model}", nameof(CarController));

            // Return a 500 status code to the client
            return StatusCode(500, "Internal server error: A critical error occurred during car update.");
        }        
    }
}
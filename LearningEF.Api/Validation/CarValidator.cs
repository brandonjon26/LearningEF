using FluentValidation;
using LearningEF.Api.Models;

namespace LearningEF.Api.Validation
{
    // The validator must inherit from AbstractValidator<T>, where T is the model we are validating
    public class CarValidator : AbstractValidator<Car>
    {
        public CarValidator()
        {
            // Rule 1: Make
            RuleFor(car => car.Make)
                .NotEmpty().WithMessage("Make is required.")
                .Length(2, 50).WithMessage("Make must be between 2 and 50 characters.");

            // Rule 2: Model
            RuleFor(car => car.Model)
                .NotEmpty().WithMessage("Model is required.")
                .Length(2, 50).WithMessage("Model must be between 2 and 50 characters.");

            // Rule 3: Year
            RuleFor(car => car.Year)
                .InclusiveBetween(1900, DateTime.Now.Year + 1)
                .WithMessage($"Year must be between 1900 and the current year ({DateTime.Now.Year + 1}).");

            // Rule 4: Color
            RuleFor(car => car.Color)
                .NotEmpty().WithMessage("Color is required.");

            // Rule 5: Price
            RuleFor(car => car.Price)
                .GreaterThanOrEqualTo(0).WithMessage("Price cannot be negative.")
                .PrecisionScale(18, 2, true)
                .WithMessage("Price must be a valid number with a maximum of two decimal places.");
        }
    }
}

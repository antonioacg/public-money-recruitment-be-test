using FluentValidation;
using VacationRental.Api.Models;

namespace VacationRental.Application.Features.Rentals.AddRental.UseCase.Validator
{
    internal class AddRentalInputValidator : AbstractValidator<AddRentalInput>
    {
        public AddRentalInputValidator()
        {
            RuleFor(i => i.Units)
                .GreaterThan(0)
                .WithMessage(i => $"{nameof(i.Units)} with value {i.Units} must be greater than 0");
        }
    }
}

using FluentValidation;
using VacationRental.Application.Features.Rentals.AddRental.Domain;

namespace VacationRental.Application.Features.Rentals.AddRental.UseCase.Validator;

internal class AddRentalInputValidator : AbstractValidator<AddRentalInput>
{
    public AddRentalInputValidator()
    {
        RuleFor(i => i.Units)
            .NotNull()
            .GreaterThan(0)
            .WithMessage(i => $"{nameof(i.Units)} with value {i.Units} must be greater than 0");

        RuleFor(i => i.PreparationTimeInDays)
            .GreaterThanOrEqualTo(0)
            .Unless(i => i.PreparationTimeInDays is null)
            .WithMessage(i => $"{nameof(i.PreparationTimeInDays)} with value {i.PreparationTimeInDays} must be greater than or equal 0");
    }
}

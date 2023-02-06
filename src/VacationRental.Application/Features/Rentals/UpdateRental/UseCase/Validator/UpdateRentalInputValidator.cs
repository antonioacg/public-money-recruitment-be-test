using FluentValidation;
using VacationRental.Application.Features.Rentals.UpdateRental.Domain;

namespace VacationRental.Application.Features.Rentals.UpdateRental.UseCase.Validator;

internal class UpdateRentalInputValidator : AbstractValidator<UpdateRentalInput>
{
    public UpdateRentalInputValidator()
    {
        RuleFor(i => i.RentalId)
            .NotNull()
            .GreaterThan(0)
            .WithMessage(i => $"{nameof(i.RentalId)} with value {i.RentalId} must be greater than 0");

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

using FluentValidation;
using VacationRental.Application.Features.Bookings.AddBooking.Domain;

namespace VacationRental.Application.Features.Bookings.AddBooking.UseCase.Validator;

internal class AddBookingInputValidator : AbstractValidator<AddBookingInput>
{
    public AddBookingInputValidator()
    {
        ClassLevelCascadeMode = CascadeMode.Stop;

        RuleFor(i => i.Nights)
            .GreaterThan(0)
            .WithMessage(i => $"{nameof(i.Nights)} with value {i.Nights} must be greater than 0");

        RuleFor(i => i.Start)
            .NotEmpty()
            .WithMessage(i => $"{nameof(i.Start)} must have non empty value")
            .GreaterThan(DateTimeOffset.Now)
            .WithMessage(i => $"{nameof(i.Start)} with value {i.Start} must be after today");

        RuleFor(i => i.RentalId)
            .GreaterThan(0)
            .WithMessage(i => $"{nameof(i.Nights)} with value {i.Nights} must be greater than 0");
    }
}

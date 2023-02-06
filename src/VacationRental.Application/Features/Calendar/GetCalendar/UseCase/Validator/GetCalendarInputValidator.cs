using FluentValidation;
using VacationRental.Application.Features.Calendar.GetCalendar.Domain;

namespace VacationRental.Application.Features.Calendar.GetCalendar.UseCase.Validator;

internal class GetCalendarInputValidator : AbstractValidator<GetCalendarInput>
{
    public GetCalendarInputValidator()
    {
        ClassLevelCascadeMode = CascadeMode.Stop;

        RuleFor(i => i.Nights)
            .NotNull()
            .GreaterThan(0)
            .WithMessage(i => $"{nameof(i.Nights)} with value {i.Nights} must be greater than 0");

        RuleFor(i => i.Start)
            .NotEmpty()
            .WithMessage(i => $"{nameof(i.Start)} must have non empty value");

        RuleFor(i => i.RentalId)
            .NotNull()
            .GreaterThan(0)
            .WithMessage(i => $"{nameof(i.RentalId)} with value {i.RentalId} must be greater than 0");
    }
}

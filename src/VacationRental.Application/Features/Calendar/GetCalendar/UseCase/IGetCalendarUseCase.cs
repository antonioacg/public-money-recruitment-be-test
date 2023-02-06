using VacationRental.Application.Features.Calendar.GetCalendar.Domain;
using VacationRental.Application.Shared.Domain.Exceptions;

namespace VacationRental.Application.Features.Calendar.GetCalendar.UseCase;

public interface IGetCalendarUseCase
{
    /// <exception cref="DataContractValidationException"></exception>
    /// <exception cref="RentalNotFoundException"></exception>
    /// <exception cref="RentalOverbookingException"></exception>
    Task<Domain.Models.Calendar> ExecuteAsync(GetCalendarInput input, CancellationToken cancellationToken = default);
}

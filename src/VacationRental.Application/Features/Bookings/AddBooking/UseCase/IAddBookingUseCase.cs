using VacationRental.Application.Features.Bookings.AddBooking.Domain;
using VacationRental.Application.Shared.Domain.Exceptions;
using VacationRental.Application.Shared.Domain.Models;

namespace VacationRental.Application.Features.Bookings.AddBooking.UseCase;

public interface IAddBookingUseCase
{
    /// <exception cref="DataContractValidationException"></exception>
    /// <exception cref="RentalNotFoundException"></exception>
    Task<Booking> ExecuteAsync(AddBookingInput input, CancellationToken cancellationToken = default);
}

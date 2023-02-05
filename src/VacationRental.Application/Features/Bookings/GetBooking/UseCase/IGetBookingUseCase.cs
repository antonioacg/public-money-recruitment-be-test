using VacationRental.Application.Shared.Domain.Models;

namespace VacationRental.Application.Features.Bookings.GetBooking.UseCase;

public interface IGetBookingUseCase
{
    /// <exception cref="ArgumentException"></exception>
    Task<Booking?> ExecuteAsync(int? id, CancellationToken cancellationToken = default);
}

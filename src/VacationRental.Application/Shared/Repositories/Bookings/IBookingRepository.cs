using VacationRental.Application.Shared.Domain.Models;

namespace VacationRental.Application.Shared.Repositories.Bookings;

internal interface IBookingRepository
{
    Task<Booking?> GetAsync(int id, CancellationToken cancellationToken = default);
    Task<IEnumerable<Booking>> GetByRentalAsync(int rentalId, CancellationToken cancellationToken = default);
    Task AddAsync(Booking booking, CancellationToken cancellationToken = default);
}

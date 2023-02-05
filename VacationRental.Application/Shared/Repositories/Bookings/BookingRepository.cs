using System.Collections.Concurrent;
using VacationRental.Application.Shared.Domain.Models;

namespace VacationRental.Application.Shared.Repositories.Bookings;

internal class BookingRepository : IBookingRepository
{
    private readonly ConcurrentDictionary<int, Booking> _bookings;

    public BookingRepository(ConcurrentDictionary<int, Booking> bookings)
    {
        _bookings = bookings;
    }

    public Task<Booking?> GetAsync(int id, CancellationToken cancellationToken = default)
    {
        _bookings.TryGetValue(id, out var booking);
        return Task.FromResult(booking);
    }

    public async Task<IEnumerable<Booking>> GetByRentalAsync(int rentalId, CancellationToken cancellationToken = default)
    {
        // Wrapped around task because enumerating values uses ConcurrentDictionary internal locking mechanism
        var rentalBookings = await Task.Run(() => _bookings.Values.Where(r => r.RentalId == rentalId), cancellationToken);
        return rentalBookings;
    }

    public Task AddAsync(Booking booking, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(booking, nameof(booking));
        if (booking.Id != default) throw new ArgumentException($"{nameof(booking)} has an already set Id");

        int id;
        do
        {
            id = _bookings.Keys.DefaultIfEmpty(0).Max() + 1;
            booking.Id = id;
        } while (!cancellationToken.IsCancellationRequested && !_bookings.TryAdd(id, booking));
        return Task.CompletedTask;
    }
}

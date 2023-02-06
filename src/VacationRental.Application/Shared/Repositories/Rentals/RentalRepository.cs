using System.Collections.Concurrent;
using VacationRental.Application.Shared.Domain.Models;

namespace VacationRental.Application.Shared.Repositories.Rentals;

public class RentalRepository : IRentalRepository
{
    private readonly ConcurrentDictionary<int, Rental> _rentals;

    public RentalRepository(ConcurrentDictionary<int, Rental> rentals)
    {
        _rentals = rentals;
    }

    public Task<Rental?> GetAsync(int id, CancellationToken cancellationToken = default)
    {
        _rentals.TryGetValue(id, out var rental);
        return Task.FromResult(rental);
    }

    public Task AddAsync(Rental rental, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(rental, nameof(rental));
        if (rental.Id != default) throw new ArgumentException($"{nameof(rental)} has an already set Id");

        int id;
        do
        {
            id = _rentals.Keys.DefaultIfEmpty(0).Max() + 1;
            rental.Id = id;
        } while (!cancellationToken.IsCancellationRequested && !_rentals.TryAdd(id, rental));
        return Task.CompletedTask;
    }
}

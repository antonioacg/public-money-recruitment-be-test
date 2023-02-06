using VacationRental.Application.Shared.Domain.Models;

namespace VacationRental.Application.Shared.Repositories.Rentals;

internal interface IRentalRepository
{
    Task<Rental?> GetAsync(int id, CancellationToken cancellationToken = default);
    Task AddAsync(Rental rental, CancellationToken cancellationToken = default);
    Task UpdateAsync(Rental rental, CancellationToken cancellationToken = default);
}

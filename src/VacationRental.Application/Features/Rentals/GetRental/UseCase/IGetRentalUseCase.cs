using VacationRental.Application.Shared.Domain.Models;

namespace VacationRental.Application.Features.Rentals.GetRental.UseCase;

public interface IGetRentalUseCase
{
    /// <exception cref="ArgumentException"></exception>
    Task<Rental?> ExecuteAsync(int? id, CancellationToken cancellationToken = default);
}

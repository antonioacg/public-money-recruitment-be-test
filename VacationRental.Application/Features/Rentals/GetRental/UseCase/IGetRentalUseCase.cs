using VacationRental.Application.Shared.Domain.Models;

namespace VacationRental.Application.Features.Rentals.GetRental.UseCase;

public interface IGetRentalUseCase
{
    Task<Rental?> ExecuteAsync(int? id, CancellationToken cancellationToken = default);
}

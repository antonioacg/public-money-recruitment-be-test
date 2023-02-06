using VacationRental.Application.Features.Rentals.AddRental.Domain;
using VacationRental.Application.Shared.Domain.Exceptions;
using VacationRental.Application.Shared.Domain.Models;

namespace VacationRental.Application.Features.Rentals.AddRental.UseCase;

public interface IAddRentalUseCase
{
    /// <exception cref="DataContractValidationException"></exception>
    Task<Rental> ExecuteAsync(AddRentalInput input, CancellationToken cancellationToken = default);
}

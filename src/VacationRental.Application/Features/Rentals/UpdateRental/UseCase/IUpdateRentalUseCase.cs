using VacationRental.Application.Features.Rentals.UpdateRental.Domain;
using VacationRental.Application.Shared.Domain.Exceptions;
using VacationRental.Application.Shared.Domain.Models;

namespace VacationRental.Application.Features.Rentals.UpdateRental.UseCase;

public interface IUpdateRentalUseCase
{
    /// <exception cref="DataContractValidationException"></exception>
    /// <exception cref="RentalNotFoundException"></exception>
    Task<Rental> ExecuteAsync(UpdateRentalInput input, CancellationToken cancellationToken = default);
}

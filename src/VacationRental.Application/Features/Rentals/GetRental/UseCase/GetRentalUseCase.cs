using Microsoft.Extensions.Logging;
using VacationRental.Application.Shared.Domain.Models;
using VacationRental.Application.Shared.Repositories.Rentals;

namespace VacationRental.Application.Features.Rentals.GetRental.UseCase;

internal class GetRentalUseCase : IGetRentalUseCase
{
    private readonly ILogger<GetRentalUseCase> _logger;
    private readonly IRentalRepository _rentalRepository;

    public GetRentalUseCase(ILogger<GetRentalUseCase> logger, IRentalRepository rentalRepository)
    {
        _logger = logger;
        _rentalRepository = rentalRepository;
    }

    public async Task<Rental?> ExecuteAsync(int? id, CancellationToken cancellationToken = default)
    {
        _logger.LogDebug("Starting {useCase}", nameof(GetRentalUseCase));

        if (id is null or <= 0) throw new ArgumentException($"{id} is invalid");

        var rental = await _rentalRepository.GetAsync(id.Value, cancellationToken);

        _logger.LogDebug("Finishing {useCase}", nameof(GetRentalUseCase));
        return rental;
    }
}

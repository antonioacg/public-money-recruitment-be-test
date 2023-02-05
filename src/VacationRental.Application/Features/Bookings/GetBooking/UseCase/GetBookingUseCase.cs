using Microsoft.Extensions.Logging;
using VacationRental.Application.Shared.Domain.Models;
using VacationRental.Application.Shared.Repositories.Bookings;

namespace VacationRental.Application.Features.Bookings.GetBooking.UseCase;

internal class GetBookingUseCase : IGetBookingUseCase
{
    private readonly ILogger<GetBookingUseCase> _logger;
    private readonly IBookingRepository _bookingRepository;

    public GetBookingUseCase(ILogger<GetBookingUseCase> logger, IBookingRepository bookingRepository)
    {
        _logger = logger;
        _bookingRepository = bookingRepository;
    }

    public async Task<Booking?> ExecuteAsync(int? id, CancellationToken cancellationToken = default)
    {
        _logger.LogDebug("Starting {useCase}", nameof(GetBookingUseCase));

        if (id is null or <= 0) throw new ArgumentException($"{id} is invalid");

        var booking = await _bookingRepository.GetAsync(id.Value, cancellationToken);

        _logger.LogDebug("Finishing {useCase}", nameof(GetBookingUseCase));
        return booking;
    }
}

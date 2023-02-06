using FluentValidation;
using Microsoft.Extensions.Logging;
using VacationRental.Application.Features.Rentals.UpdateRental.Domain;
using VacationRental.Application.Shared.Domain.Exceptions;
using VacationRental.Application.Shared.Domain.Models;
using VacationRental.Application.Shared.Extensions;
using VacationRental.Application.Shared.Repositories.Bookings;
using VacationRental.Application.Shared.Repositories.Rentals;

namespace VacationRental.Application.Features.Rentals.UpdateRental.UseCase;

internal class UpdateRentalUseCase : IUpdateRentalUseCase
{
    private readonly ILogger<UpdateRentalUseCase> _logger;
    private readonly IValidator<UpdateRentalInput> _validator;
    private readonly IRentalRepository _rentalRepository;
    private readonly IBookingRepository _bookingRepository;
    private readonly Dictionary<Booking, int> _bookedUnitIds = new();
    private Rental? _rental;

    public UpdateRentalUseCase(ILogger<UpdateRentalUseCase> logger, IValidator<UpdateRentalInput> validator,
        IRentalRepository rentalRepository, IBookingRepository bookingRepository)
    {
        _logger = logger;
        _validator = validator;
        _rentalRepository = rentalRepository;
        _bookingRepository = bookingRepository;
    }

    public async Task<Rental> ExecuteAsync(UpdateRentalInput input, CancellationToken cancellationToken = default)
    {
        _logger.LogDebug("Starting {useCase}", nameof(UpdateRentalUseCase));

        await EnsureValidInput(input, cancellationToken);

        // We will attempt to update, and only save if no overlap happens
        _rental!.Units = input.Units!.Value;
        _rental!.PreparationTimeInDays = input.PreparationTimeInDays!.Value;

        var rentalBookings = await _bookingRepository.GetByRentalAsync(_rental.Id, cancellationToken);

        // Remove bookings from the past from analysis
        rentalBookings = rentalBookings
            .Where(b =>
            {
                var bookingStart = b.Start;
                var bookingPreparationTimeEnd = bookingStart.AddDays(b.Nights + _rental!.PreparationTimeInDays);
                return bookingPreparationTimeEnd > DateTime.Now.Date;
            })
            .ToList();

        var dates = rentalBookings.SelectMany(b => new[] { b.Start, b.Start.AddDays(b.Nights + _rental!.PreparationTimeInDays) }).Distinct();
        foreach (var date in dates)
        {
            foreach (var rentalBooking in rentalBookings)
            {
                var bookingStart = rentalBooking.Start;
                var newBookingPreparationTimeEnd = bookingStart.AddDays(rentalBooking.Nights + _rental!.PreparationTimeInDays);

                if (newBookingPreparationTimeEnd < date) ReleaseRentedUnitId(rentalBooking);
                if (bookingStart <= date && date.Date < newBookingPreparationTimeEnd) GetRentedUnitId(rentalBooking);
            }
        }

        _logger.LogInformation("Updating Rental: {@rental}", _rental);
        await _rentalRepository.UpdateAsync(_rental, cancellationToken);

        _logger.LogDebug("Finishing {useCase}", nameof(UpdateRentalUseCase));
        return _rental;
    }

    private async Task EnsureValidInput(UpdateRentalInput input, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(input, nameof(input));

        var validationResult = await _validator.ValidateAsync(input, cancellationToken);
        if (!validationResult.IsValid)
        {
            _logger.Log(validationResult.Errors);
            throw new DataContractValidationException(validationResult);
        }

        _rental = await _rentalRepository.GetAsync(input.RentalId!.Value, cancellationToken);
        if (_rental is null)
        {
            _logger.LogError("Could not find related rental with {rentalId}", input.RentalId);
            throw new RentalNotFoundException();
        }
    }
    private int GetRentedUnitId(Booking booking)
    {
        if (_bookedUnitIds.TryGetValue(booking, out var bookedUnitId))
            return bookedUnitId;

        bookedUnitId = ReserveRentUnitId(booking);
        return bookedUnitId;
    }

    private int ReserveRentUnitId(Booking booking)
    {
        if (_bookedUnitIds.Count == _rental!.Units)
            throw new RentalOverbookingException();

        var bookedUnitId = Enumerable
            .Range(1, _rental!.Units)
            .FirstOrDefault(id => !_bookedUnitIds.ContainsValue(id));

        _bookedUnitIds.Add(booking, bookedUnitId);
        return bookedUnitId;
    }

    private void ReleaseRentedUnitId(Booking booking)
    {
        if (!_bookedUnitIds.TryGetValue(booking, out _))
            return;

        _bookedUnitIds.Remove(booking);
    }
}

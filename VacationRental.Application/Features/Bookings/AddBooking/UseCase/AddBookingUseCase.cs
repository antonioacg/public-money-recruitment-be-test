using FluentValidation;
using Microsoft.Extensions.Logging;
using VacationRental.Application.Features.Bookings.AddBooking.Domain;
using VacationRental.Application.Features.Bookings.AddBooking.UseCase.Mapper;
using VacationRental.Application.Shared.Domain.Exceptions;
using VacationRental.Application.Shared.Domain.Models;
using VacationRental.Application.Shared.Extensions;
using VacationRental.Application.Shared.Repositories.Bookings;
using VacationRental.Application.Shared.Repositories.Rentals;

namespace VacationRental.Application.Features.Bookings.AddBooking.UseCase;

internal class AddBookingUseCase : IAddBookingUseCase
{
    private readonly ILogger<AddBookingUseCase> _logger;
    private readonly IValidator<AddBookingInput> _validator;
    private readonly IBookingRepository _bookingRepository;
    private readonly IRentalRepository _rentalRepository;
    private Rental? _rental;

    public AddBookingUseCase(ILogger<AddBookingUseCase> logger, IValidator<AddBookingInput> validator,
        IBookingRepository bookingRepository, IRentalRepository rentalRepository)
    {
        _logger = logger;
        _validator = validator;
        _bookingRepository = bookingRepository;
        _rentalRepository = rentalRepository;
    }

    public async Task<Booking> ExecuteAsync(AddBookingInput input, CancellationToken cancellationToken = default)
    {
        _logger.LogDebug("Starting {useCase}", nameof(AddBookingUseCase));

        await EnsureValidInput(input, cancellationToken);

        var newBooking = input.ToBooking();
        var newBookingStart = newBooking.Start;
        var newBookingEnd = newBookingStart.AddDays(newBooking.Nights + _rental!.PreparationTimeInDays);

        var rentalBookings = await _bookingRepository.GetByRentalAsync(newBooking.RentalId, cancellationToken);

        var unitsUsed = rentalBookings.Count(rentalBooking =>
        {
            var rentalBookingStart = rentalBooking.Start;
            var rentalBookingEnd = rentalBookingStart.AddDays(rentalBooking.Nights + _rental.PreparationTimeInDays);

            if (newBookingEnd < rentalBookingStart) return false;
            if (newBookingStart > rentalBookingEnd) return false;
            return true;
        });

        if (unitsUsed >= _rental.Units)
            throw new RentalUnitsNotAvailableException();

        //for (var i = 0; i < newBooking.Nights; i++)
        //{
        //    var count = 0;
        //    foreach (var rentalBooking in rentalBookings)
        //    {
        //        var rentalBookingStart = rentalBooking.Start;
        //        var rentalBookingEnd = rentalBookingStart.AddDays(rentalBooking.Nights + _rental.PreparationTimeInDays);

        //        if ((rentalBookingStart <= newBookingStart.Date && newBookingStart.Date < rentalBookingEnd)
        //            || (rentalBookingStart < newBookingEnd && newBookingEnd <= rentalBookingEnd)
        //            || (rentalBookingStart > newBookingStart && newBookingEnd > rentalBookingEnd))
        //            count++;
        //    }
        //    if (count >= _rental.Units)
        //        throw new RentalUnitsNotAvailableException();
        //}

        _logger.LogInformation("Adding new Booking: {@booking}", newBooking);
        await _bookingRepository.AddAsync(newBooking, cancellationToken);

        _logger.LogDebug("Finishing {useCase}", nameof(AddBookingUseCase));
        return newBooking;
    }

    private async Task EnsureValidInput(AddBookingInput input, CancellationToken cancellationToken)
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
}

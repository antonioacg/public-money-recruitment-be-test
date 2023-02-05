using FluentValidation;
using Microsoft.Extensions.Logging;
using VacationRental.Application.Features.Calendar.GetCalendar.Domain;
using VacationRental.Application.Features.Calendar.GetCalendar.Domain.Models;
using VacationRental.Application.Shared.Domain.Exceptions;
using VacationRental.Application.Shared.Domain.Models;
using VacationRental.Application.Shared.Extensions;
using VacationRental.Application.Shared.Repositories.Bookings;
using VacationRental.Application.Shared.Repositories.Rentals;

namespace VacationRental.Application.Features.Calendar.GetCalendar.UseCase;

internal class GetCalendarUseCase : IGetCalendarUseCase
{
    private readonly ILogger<GetCalendarUseCase> _logger;
    private readonly IValidator<GetCalendarInput> _validator;
    private readonly IBookingRepository _bookingRepository;
    private readonly IRentalRepository _rentalRepository;
    private Rental? _rental;
    private readonly Dictionary<Booking, int> _bookedUnitIds = new Dictionary<Booking, int>();

    public GetCalendarUseCase(ILogger<GetCalendarUseCase> logger, IValidator<GetCalendarInput> validator,
        IBookingRepository bookingRepository, IRentalRepository rentalRepository)
    {
        _logger = logger;
        _validator = validator;
        _bookingRepository = bookingRepository;
        _rentalRepository = rentalRepository;
    }

    public async Task<Domain.Models.Calendar> ExecuteAsync(GetCalendarInput input, CancellationToken cancellationToken = default)
    {
        _logger.LogDebug("Starting {useCase}", nameof(GetCalendarUseCase));

        await EnsureValidInput(input, cancellationToken);

        var rentalBookings = await _bookingRepository.GetByRentalAsync(input.RentalId!.Value, cancellationToken);

        var nights = input.Nights!.Value;
        var preparationDays = _rental!.PreparationTimeInDays;

        var startDate = input.Start!.Value.Date;
        var endDate = startDate.AddDays(nights);

        rentalBookings = rentalBookings
            .Where(b =>
            {
                var bookingStart = b.Start;
                var bookingPreparationTimeEnd = bookingStart.AddDays(b.Nights + preparationDays);

                if (endDate < bookingStart) return false;
                if (startDate > bookingPreparationTimeEnd) return false;
                return true;
            })
            .ToList();

        var calendarDates = Enumerable
            .Range(0, nights)
            .Select(i => startDate.AddDays(i))
            .Select(date =>
            {
                var calendarDate = new CalendarDate { Date = date };
                foreach (var rentalBooking in rentalBookings)
                {
                    var bookingStart = rentalBooking.Start;
                    var bookingNightsEnd = bookingStart.AddDays(rentalBooking.Nights);
                    var bookingPreparationTimeEnd = bookingNightsEnd.AddDays(_rental.PreparationTimeInDays);

                    if (bookingPreparationTimeEnd < date) ReleaseRentedUnitId(rentalBooking);

                    if (bookingStart <= date && date < bookingNightsEnd)
                    {
                        calendarDate.Bookings.Add(new CalendarBooking
                        {
                            Id = rentalBooking.Id,
                            Unit = GetRentedUnitId(rentalBooking)
                        });
                    }

                    if (bookingNightsEnd <= date.Date && date.Date < bookingPreparationTimeEnd)
                    {
                        calendarDate.PreparationTimes.Add(new CalendarPreparationTime
                        {
                            Unit = GetRentedUnitId(rentalBooking)
                        });
                    }
                }
                return calendarDate;
            })
            .ToList();

        var result = new Domain.Models.Calendar
        {
            RentalId = _rental!.Id,
            Dates = calendarDates
        };

        _logger.LogDebug("Finishing {useCase}", nameof(GetCalendarUseCase));
        return result;
    }

    private async Task EnsureValidInput(GetCalendarInput input, CancellationToken cancellationToken)
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

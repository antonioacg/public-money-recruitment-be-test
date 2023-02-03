using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using VacationRental.Api.Models;

namespace VacationRental.Api.Controllers
{
    [Route("api/v1/calendar")]
    [ApiController]
    public class CalendarController : ControllerBase
    {
        private readonly IDictionary<int, RentalViewModel> _rentals;
        private readonly IDictionary<int, BookingViewModel> _bookings;

        public CalendarController(
            IDictionary<int, RentalViewModel> rentals,
            IDictionary<int, BookingViewModel> bookings)
        {
            _rentals = rentals;
            _bookings = bookings;
        }

        [HttpGet]
        public CalendarViewModel Get(int rentalId, DateTime start, int nights)
        {
            if (nights < 0)
                throw new ApplicationException("Nights must be positive");
            if (!_rentals.TryGetValue(rentalId, out var rental))
                throw new ApplicationException("Rental not found");

            var result = new CalendarViewModel
            {
                RentalId = rentalId,
                Dates = new List<CalendarDateViewModel>()
            };
            var bookingUnitMap = new Dictionary<BookingViewModel, int>();
            for (var i = 0; i < nights; i++)
            {
                var date = new CalendarDateViewModel
                {
                    Date = start.Date.AddDays(i)
                };

                foreach (var booking in _bookings.Values)
                {
                    if (booking.RentalId != rentalId) continue;


                    if (!bookingUnitMap.TryGetValue(booking, out var bookingUnit))
                    {
                        if (bookingUnitMap.Count == rental.Units)
                            throw new Exception("overbooking");

                        bookingUnit = bookingUnitMap.Count + 1;
                        bookingUnitMap.Add(booking, bookingUnit);
                    }

                    var bookingStart = booking.Start;
                    var bookingNightsEnd = bookingStart.AddDays(booking.Nights);
                    var bookingPreparationTimeEnd = bookingNightsEnd.AddDays(rental.PreparationTimeInDays);

                    if (bookingStart <= date.Date && date.Date < bookingNightsEnd)
                    {
                        date.Bookings.Add(new CalendarBookingViewModel
                        {
                            Id = booking.Id,
                            Unit = bookingUnit
                        });
                    }
                    if (bookingNightsEnd <= date.Date && date.Date < bookingPreparationTimeEnd)
                    {
                        date.PreparationTimes.Add(new CalendarPreparationTimeViewModel
                        {
                            Unit = bookingUnit
                        });
                    }
                }

                result.Dates.Add(date);
            }

            return result;
        }
    }
}

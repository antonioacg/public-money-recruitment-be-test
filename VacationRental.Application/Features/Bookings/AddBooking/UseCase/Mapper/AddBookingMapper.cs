using VacationRental.Application.Features.Bookings.AddBooking.Domain;
using VacationRental.Application.Shared.Domain.Models;

namespace VacationRental.Application.Features.Bookings.AddBooking.UseCase.Mapper;

internal static class AddBookingMapper
{
    internal static Booking ToBooking(this AddBookingInput input)
    {
        var booking = new Booking
        {
            RentalId = input.RentalId!.Value,
            Start = input.Start!.Value.Date,
            Nights = input.Nights!.Value
        };
        return booking;
    }
}

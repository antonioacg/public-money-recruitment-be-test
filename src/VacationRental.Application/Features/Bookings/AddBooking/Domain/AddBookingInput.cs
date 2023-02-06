namespace VacationRental.Application.Features.Bookings.AddBooking.Domain;

public class AddBookingInput
{
    public int? RentalId { get; set; }
    public DateTimeOffset? Start { get; set; }
    public int? Nights { get; set; }
}

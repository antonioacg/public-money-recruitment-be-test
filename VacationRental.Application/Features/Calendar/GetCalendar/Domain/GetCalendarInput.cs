namespace VacationRental.Application.Features.Calendar.GetCalendar.Domain;

public class GetCalendarInput
{
    public int? RentalId { get; set; }
    public DateTimeOffset? Start { get; set; }
    public int? Nights { get; set; }
}

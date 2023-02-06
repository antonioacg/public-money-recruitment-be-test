namespace VacationRental.Application.Features.Calendar.GetCalendar.Domain.Models;

public class Calendar
{
    public int RentalId { get; set; }
    public List<CalendarDate> Dates { get; set; } = new();
}

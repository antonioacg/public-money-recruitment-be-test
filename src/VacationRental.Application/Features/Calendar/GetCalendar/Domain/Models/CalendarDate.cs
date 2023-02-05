namespace VacationRental.Application.Features.Calendar.GetCalendar.Domain.Models;

public class CalendarDate
{
    public DateTime Date { get; set; }
    public List<CalendarBooking> Bookings { get; set; } = new();
    public List<CalendarPreparationTime> PreparationTimes { get; set; } = new();
}
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using VacationRental.Application.Features.Calendar.GetCalendar.Domain.Models;
using VacationRental.Application.Tests.Extensions;

namespace VacationRental.Application.Tests.Features.Calendar.GetCalendar.Tables;

internal class GetCalendarOutputTable
{
    public int? RentalId { get; set; }
    public string? Date { get; set; }
    public DateTimeOffset? DateDateTimeOffset => string.IsNullOrWhiteSpace(Date) ? null :
        StepArgumentTransformationExtensions.ConvertToRelativeDate(Date ?? "");
    public int? BookingId { get; set; }
    public int? BookingUnit { get; set; }
    public int? PreparationTimeUnit { get; set; }

    public static Application.Features.Calendar.GetCalendar.Domain.Models.Calendar ToCalendar(
        IEnumerable<GetCalendarOutputTable> set) =>
        set.GroupBy(x => x.RentalId)
            .Select(rentalGroup => new Application.Features.Calendar.GetCalendar.Domain.Models.Calendar
            {
                RentalId = rentalGroup.Key!.Value,
                Dates = rentalGroup.GroupBy(g => g.DateDateTimeOffset).Select(dateGroup => new CalendarDate
                {
                    Date = dateGroup.Key!.Value.Date,
                    Bookings = dateGroup.Where(x => x.BookingId > 0).Select(x => new CalendarBooking
                    {
                        Id = x.BookingId!.Value,
                        Unit = x.BookingUnit!.Value,
                    }).ToList(),
                    PreparationTimes = dateGroup.Where(x => x.PreparationTimeUnit > 0).Select(x => new CalendarPreparationTime
                    {
                        Unit = x.PreparationTimeUnit!.Value,
                    }).ToList()
                }).ToList()
            })
            .First();
}

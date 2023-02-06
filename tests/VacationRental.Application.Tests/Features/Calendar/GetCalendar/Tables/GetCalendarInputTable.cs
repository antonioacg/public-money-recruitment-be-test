using System;
using VacationRental.Application.Features.Calendar.GetCalendar.Domain;
using VacationRental.Application.Tests.Extensions;

namespace VacationRental.Application.Tests.Features.Calendar.GetCalendar.Tables
{
    internal class GetCalendarInputTable
    {
        public int? RentalId { get; set; }
        public string? Start { get; set; }
        public DateTimeOffset? StartDateTimeOffset => string.IsNullOrWhiteSpace(Start) ? null :
            StepArgumentTransformationExtensions.ConvertToRelativeDate(Start ?? "");
        public int? Nights { get; set; }

        public GetCalendarInput ToCalendarInput() =>
            new()
            {
                RentalId = RentalId,
                Start = StartDateTimeOffset,
                Nights = Nights,
            };
    }
}

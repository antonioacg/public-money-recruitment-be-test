using System;
using VacationRental.Application.Features.Bookings.AddBooking.Domain;
using VacationRental.Application.Tests.Extensions;

namespace VacationRental.Application.Tests.Features.Bookings.AddBooking.Tables
{
    internal class AddBookingInputTable
    {
        public int? RentalId { get; set; }
        public string? Start { get; set; }
        public DateTimeOffset? StartDateTimeOffset => string.IsNullOrWhiteSpace(Start) ? null :
            StepArgumentTransformationExtensions.ConvertToRelativeDate(Start ?? "");
        public int? Nights { get; set; }

        public AddBookingInput ToAddBookingInput() =>
            new()
            {
                RentalId = RentalId,
                Start = StartDateTimeOffset,
                Nights = Nights,
            };
    }
}

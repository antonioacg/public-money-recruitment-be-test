using System;
using VacationRental.Application.Shared.Domain.Models;
using VacationRental.Application.Tests.Extensions;

namespace VacationRental.Application.Tests.Shared.Tables;

internal class BookingTable
{
    public int? Id { get; set; }
    public int? RentalId { get; set; }
    public string? Start { get; set; }
    public DateTimeOffset? StartDateTimeOffset => string.IsNullOrWhiteSpace(Start) ? null :
        StepArgumentTransformationExtensions.ConvertToRelativeDate(Start ?? "");
    public int? Nights { get; set; }

    public Booking ToBooking()
    {
        return new Booking
        {
            Id = Id!.Value,
            RentalId = RentalId!.Value,
            Start = StartDateTimeOffset!.Value.Date,
            Nights = Nights!.Value,
        };
    }
}

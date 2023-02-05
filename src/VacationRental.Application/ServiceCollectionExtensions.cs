using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics.CodeAnalysis;
using VacationRental.Application.Features.Bookings.AddBooking;
using VacationRental.Application.Features.Bookings.GetBooking;
using VacationRental.Application.Features.Calendar.GetCalendar;
using VacationRental.Application.Features.Rentals.AddRental;
using VacationRental.Application.Features.Rentals.GetRental;
using VacationRental.Application.Shared.Repositories;

namespace VacationRental.Application;

[ExcludeFromCodeCoverage]
public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddVacationRentalApplication(this IServiceCollection services)
    {
        ArgumentNullException.ThrowIfNull(services, nameof(services));

        services.AddRepositories();

        services.AddAddRental();
        services.AddGetRental();

        services.AddAddBooking();
        services.AddGetBooking();

        services.AddGetCalendar();

        return services;
    }
}

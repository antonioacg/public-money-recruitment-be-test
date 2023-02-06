using Microsoft.Extensions.DependencyInjection;
using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;
using VacationRental.Application.Shared.Domain.Models;

namespace VacationRental.Application.Shared.Repositories.Bookings;

[ExcludeFromCodeCoverage]
internal static class ServiceCollectionExtensions
{
    internal static IServiceCollection AddBookingRepository(this IServiceCollection services)
    {
        ArgumentNullException.ThrowIfNull(services, nameof(services));

        services.AddScoped<IBookingRepository, BookingRepository>();
        services.AddSingleton(new ConcurrentDictionary<int, Booking>());

        return services;
    }
}

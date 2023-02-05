using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.DependencyInjection;
using VacationRental.Application.Shared.Repositories.Bookings;
using VacationRental.Application.Shared.Repositories.Rentals;

namespace VacationRental.Application.Shared.Repositories;

[ExcludeFromCodeCoverage]
internal static class ServiceCollectionExtensions
{
    internal static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        ArgumentNullException.ThrowIfNull(services, nameof(services));

        services.AddRentalRepository();
        services.AddBookingRepository();

        return services;
    }
}

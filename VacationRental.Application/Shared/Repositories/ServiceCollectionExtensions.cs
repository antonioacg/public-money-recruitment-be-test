using Microsoft.Extensions.DependencyInjection;
using VacationRental.Application.Shared.Repositories.Rentals;

namespace VacationRental.Application.Shared.Repositories;

internal static class ServiceCollectionExtensions
{
    internal static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        ArgumentNullException.ThrowIfNull(services, nameof(services));

        services.AddRentalRepository();

        return services;
    }
}

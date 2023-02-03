using Microsoft.Extensions.DependencyInjection;
using VacationRental.Application.Features.Rentals.AddRental;
using VacationRental.Application.Features.Rentals.GetRental;
using VacationRental.Application.Shared.Repositories;

namespace VacationRental.Application;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddVacationRentalApplication(this IServiceCollection services)
    {
        ArgumentNullException.ThrowIfNull(services, nameof(services));

        services.AddRepositories();

        services.AddAddRental();
        services.AddGetRental();

        return services;
    }
}

using Microsoft.Extensions.DependencyInjection;
using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;
using VacationRental.Application.Shared.Domain.Models;

namespace VacationRental.Application.Shared.Repositories.Rentals;

internal static class ServiceCollectionExtensions
{
    [ExcludeFromCodeCoverage]
    internal static IServiceCollection AddRentalRepository(this IServiceCollection services)
    {
        ArgumentNullException.ThrowIfNull(services, nameof(services));

        services.AddScoped<IRentalRepository, RentalRepository>();
        services.AddSingleton(new ConcurrentDictionary<int, Rental>());

        return services;
    }
}

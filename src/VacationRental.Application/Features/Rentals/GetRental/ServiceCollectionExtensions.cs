using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics.CodeAnalysis;
using VacationRental.Application.Features.Rentals.GetRental.UseCase;

namespace VacationRental.Application.Features.Rentals.GetRental;

internal static class ServiceCollectionExtensions
{
    [ExcludeFromCodeCoverage]
    internal static IServiceCollection AddGetRental(this IServiceCollection services)
    {
        ArgumentNullException.ThrowIfNull(services, nameof(services));

        services.AddScoped<IGetRentalUseCase, GetRentalUseCase>();

        return services;
    }
}

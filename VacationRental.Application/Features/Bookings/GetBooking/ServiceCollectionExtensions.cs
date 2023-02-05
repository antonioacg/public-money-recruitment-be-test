using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.DependencyInjection;
using VacationRental.Application.Features.Bookings.GetBooking.UseCase;

namespace VacationRental.Application.Features.Bookings.GetBooking;

[ExcludeFromCodeCoverage]
internal static class ServiceCollectionExtensions
{
    internal static IServiceCollection AddGetBooking(this IServiceCollection services)
    {
        ArgumentNullException.ThrowIfNull(services, nameof(services));

        services.AddScoped<IGetBookingUseCase, GetBookingUseCase>();

        return services;
    }
}

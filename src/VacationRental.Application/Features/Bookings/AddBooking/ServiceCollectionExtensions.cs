using System.Diagnostics.CodeAnalysis;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using VacationRental.Application.Features.Bookings.AddBooking.Domain;
using VacationRental.Application.Features.Bookings.AddBooking.UseCase;
using VacationRental.Application.Features.Bookings.AddBooking.UseCase.Validator;

namespace VacationRental.Application.Features.Bookings.AddBooking;

[ExcludeFromCodeCoverage]
internal static class ServiceCollectionExtensions
{
    internal static IServiceCollection AddAddBooking(this IServiceCollection services)
    {
        ArgumentNullException.ThrowIfNull(services, nameof(services));

        services.AddScoped<IValidator<AddBookingInput>, AddBookingInputValidator>();
        services.AddScoped<IAddBookingUseCase, AddBookingUseCase>();

        return services;
    }
}

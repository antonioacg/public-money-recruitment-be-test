using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics.CodeAnalysis;
using VacationRental.Application.Features.Rentals.AddRental.Domain;
using VacationRental.Application.Features.Rentals.AddRental.UseCase;
using VacationRental.Application.Features.Rentals.AddRental.UseCase.Validator;

namespace VacationRental.Application.Features.Rentals.AddRental;

internal static class ServiceCollectionExtensions
{
    [ExcludeFromCodeCoverage]
    internal static IServiceCollection AddAddRental(this IServiceCollection services)
    {
        ArgumentNullException.ThrowIfNull(services, nameof(services));

        services.AddScoped<IValidator<AddRentalInput>, AddRentalInputValidator>();
        services.AddScoped<IAddRentalUseCase, AddRentalUseCase>();

        return services;
    }
}

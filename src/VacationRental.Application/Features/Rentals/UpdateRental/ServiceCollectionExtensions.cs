using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics.CodeAnalysis;
using VacationRental.Application.Features.Rentals.UpdateRental.Domain;
using VacationRental.Application.Features.Rentals.UpdateRental.UseCase;
using VacationRental.Application.Features.Rentals.UpdateRental.UseCase.Validator;

namespace VacationRental.Application.Features.Rentals.UpdateRental;

[ExcludeFromCodeCoverage]
internal static class ServiceCollectionExtensions
{
    internal static IServiceCollection AddUpdateRental(this IServiceCollection services)
    {
        ArgumentNullException.ThrowIfNull(services, nameof(services));

        services.AddScoped<IValidator<UpdateRentalInput>, UpdateRentalInputValidator>();
        services.AddScoped<IUpdateRentalUseCase, UpdateRentalUseCase>();

        return services;
    }
}

using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics.CodeAnalysis;
using VacationRental.Application.Features.Calendar.GetCalendar.Domain;
using VacationRental.Application.Features.Calendar.GetCalendar.UseCase;
using VacationRental.Application.Features.Calendar.GetCalendar.UseCase.Validator;

namespace VacationRental.Application.Features.Calendar.GetCalendar;

[ExcludeFromCodeCoverage]
internal static class ServiceCollectionExtensions
{
    internal static IServiceCollection AddGetCalendar(this IServiceCollection services)
    {
        ArgumentNullException.ThrowIfNull(services, nameof(services));

        services.AddScoped<IValidator<GetCalendarInput>, GetCalendarInputValidator>();
        services.AddScoped<IGetCalendarUseCase, GetCalendarUseCase>();

        return services;
    }
}

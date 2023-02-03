using FluentValidation;
using FluentValidation.Results;
using Microsoft.Extensions.Logging;
#pragma warning disable CA2254

namespace VacationRental.Application.Shared.Extensions
{
    internal static class LoggerExtensions
    {
        internal static void Log(this ILogger logger, IEnumerable<ValidationFailure> failures)
        {
            foreach (var failure in failures)
                logger.Log(failure.Severity.ToLogLevel(), failure.ErrorMessage);
        }

        private static LogLevel ToLogLevel(this Severity severity)
        {
            return severity switch
            {
                Severity.Error => LogLevel.Error,
                Severity.Warning => LogLevel.Warning,
                _ => LogLevel.Information,
            };
        }
    }
}

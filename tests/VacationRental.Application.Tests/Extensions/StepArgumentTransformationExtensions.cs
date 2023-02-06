using System;
using System.Text.RegularExpressions;
using TechTalk.SpecFlow;
using VacationRental.Application.Shared.Domain.Exceptions;

namespace VacationRental.Application.Tests.Extensions;

[Binding]
internal static class StepArgumentTransformationExtensions
{
    [StepArgumentTransformation]
    public static Exception ConvertToException(string exception)
    {
        return exception switch
        {
            "ArgumentException" => new ArgumentException(),
            "DataContractValidationException" => new DataContractValidationException(),
            "RentalNotFoundException" => new RentalNotFoundException(),
            "RentalUnitsNotAvailableException" => new RentalUnitsNotAvailableException(),
            "RentalOverbookingException" => new RentalOverbookingException(),
            _ => throw new ArgumentException($"'{exception}' is not mapped for transformation"),
        };
    }

    [StepArgumentTransformation(@"D[+-]?(?<relativeDays>\d+)(?: (?<time>.*))?")]
    public static DateTimeOffset ConvertToRelativeDate(string dateTimeText)
    {
        var regex = new Regex(@"D[+-]?(?<relativeDays>\d+)(?: (?<time>.*))?");
        var relativeDaysText = regex.Match(dateTimeText).Groups["relativeDays"].Value;

        if (!int.TryParse(relativeDaysText, out var relativeDays))
            return default;

        var dateTimeOffset = DateTimeOffset.Now;
        dateTimeOffset = dateTimeOffset.AddDays(relativeDays);

        var timeText = regex.Match(dateTimeText).Groups["time"].Value;
        if (TimeSpan.TryParse(timeText, out var time))
            dateTimeOffset = dateTimeOffset.Add(time);

        return dateTimeOffset;
    }
}

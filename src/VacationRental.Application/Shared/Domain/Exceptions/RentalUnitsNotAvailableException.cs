using System.Diagnostics.CodeAnalysis;

namespace VacationRental.Application.Shared.Domain.Exceptions;

[Serializable]
[ExcludeFromCodeCoverage]
public class RentalUnitsNotAvailableException : VacationRentalException
{
    private static string _defaultMessage = "Rental units are not available";

    public RentalUnitsNotAvailableException() : base(_defaultMessage)
    {
    }
}

using System.Diagnostics.CodeAnalysis;

namespace VacationRental.Application.Shared.Domain.Exceptions;

[Serializable]
[ExcludeFromCodeCoverage]
public class RentalNotFoundException : VacationRentalException
{
    private static string _defaultMessage = "Rental not found";

    public RentalNotFoundException() : base(_defaultMessage)
    {
    }
}

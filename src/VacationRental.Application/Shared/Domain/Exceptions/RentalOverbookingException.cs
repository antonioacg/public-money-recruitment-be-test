using System.Diagnostics.CodeAnalysis;

namespace VacationRental.Application.Shared.Domain.Exceptions;

[Serializable]
[ExcludeFromCodeCoverage]
public class RentalOverbookingException : VacationRentalException
{
    private static string _defaultMessage = "Rental units are overbooked";

    public RentalOverbookingException() : base(_defaultMessage)
    {
    }
}

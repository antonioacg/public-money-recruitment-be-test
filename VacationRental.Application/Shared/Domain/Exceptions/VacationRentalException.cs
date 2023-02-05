using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;

namespace VacationRental.Application.Shared.Domain.Exceptions;

[Serializable]
[ExcludeFromCodeCoverage]
public class VacationRentalException : Exception
{
    public VacationRentalException()
    {
    }

    protected VacationRentalException(SerializationInfo info, StreamingContext context)
    {
    }
}

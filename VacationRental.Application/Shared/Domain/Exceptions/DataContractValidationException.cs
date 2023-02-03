using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;
using FluentValidation.Results;

namespace VacationRental.Application.Shared.Domain.Exceptions;

[Serializable]
[ExcludeFromCodeCoverage]
public class DataContractValidationException : VacationRentalException
{
    public ValidationResult ValidationResult { get; }

    public DataContractValidationException(ValidationResult validationResult)
    {
        ValidationResult = validationResult;
    }

    protected DataContractValidationException(SerializationInfo info, StreamingContext context)
    {
        ValidationResult = info.GetValue(nameof(ValidationResult), typeof(ValidationResult)) as ValidationResult ??
                           new ValidationResult();
    }

    public override void GetObjectData(SerializationInfo info, StreamingContext context)
    {
        base.GetObjectData(info, context);

        info.AddValue(nameof(ValidationResult), ValidationResult);
    }
}

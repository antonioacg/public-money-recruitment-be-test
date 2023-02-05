using FluentValidation.Results;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;

namespace VacationRental.Application.Shared.Domain.Exceptions;

[Serializable]
[ExcludeFromCodeCoverage]
public class DataContractValidationException : VacationRentalException
{
    public ValidationResult ValidationResult { get; }
    public IEnumerable<string> ValidationErrorMessages =>
        ValidationResult.Errors.Select(e => e.ErrorMessage);

    public DataContractValidationException(ValidationResult validationResult)
    {
        ValidationResult = validationResult;
    }

    protected DataContractValidationException(SerializationInfo info, StreamingContext context) : base(info, context)
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

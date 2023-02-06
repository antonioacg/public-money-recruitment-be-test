namespace VacationRental.Application.Features.Rentals.UpdateRental.Domain;

public class UpdateRentalInput
{
    public int? Units { get; set; }
    public int? PreparationTimeInDays { get; set; }
    public int? RentalId { get; set; }
}

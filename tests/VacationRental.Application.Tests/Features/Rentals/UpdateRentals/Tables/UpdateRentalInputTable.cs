using VacationRental.Application.Features.Rentals.UpdateRental.Domain;

namespace VacationRental.Application.Tests.Features.Rentals.UpdateRentals.Tables
{
    internal class UpdateRentalInputTable
    {
        public int? RentalId { get; set; }
        public int? Units { get; set; }
        public int? PreparationTimeInDays { get; set; }

        public UpdateRentalInput ToUpdateRentalInput() =>
            new()
            {
                RentalId = RentalId,
                Units = Units,
                PreparationTimeInDays = PreparationTimeInDays,
            };
    }
}

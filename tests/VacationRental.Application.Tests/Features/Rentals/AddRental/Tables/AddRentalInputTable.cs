using VacationRental.Application.Features.Rentals.AddRental.Domain;

namespace VacationRental.Application.Tests.Features.Rentals.AddRental.Tables
{
    internal class AddRentalInputTable
    {
        public int? Units { get; set; }
        public int? PreparationTimeInDays { get; set; }

        public AddRentalInput ToAddRentalInput() =>
            new()
            {
                Units = Units,
                PreparationTimeInDays = PreparationTimeInDays,
            };
    }
}

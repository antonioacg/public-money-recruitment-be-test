using VacationRental.Api.Models;
using VacationRental.Application.Shared.Domain.Models;

namespace VacationRental.Application.Features.Rentals.AddRental.UseCase.Mapper
{
    internal static class AddRentalMapper
    {
        internal static Rental ToRental(this AddRentalInput input)
        {
            var rental = new Rental
            {
                Units = input.Units,
                PreparationTimeInDays = input.PreparationTimeInDays ?? 0,
            };
            return rental;
        }
    }
}

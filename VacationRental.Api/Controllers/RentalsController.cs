using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using VacationRental.Api.Models;
using VacationRental.Application.Features.Rentals.AddRental.UseCase;
using VacationRental.Application.Features.Rentals.GetRental.UseCase;
using VacationRental.Application.Shared.Domain.Exceptions;
using VacationRental.Application.Shared.Domain.Models;

namespace VacationRental.Api.Controllers
{
    [Route("api/v1/rentals")]
    [ApiController]
    public class RentalsController : ControllerBase
    {
        private readonly IDictionary<int, RentalViewModel> _rentals;

        public RentalsController(IDictionary<int, RentalViewModel> rentals)
        {
            _rentals = rentals;
        }

        [HttpGet("{rentalId:int?}")]
        [ProducesResponseType(typeof(Rental), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        public async Task<ActionResult> Get([FromServices] IGetRentalUseCase getRentalUseCase,
            [FromRoute] int? rentalId, CancellationToken cancellationToken)
        {
            var rental = await getRentalUseCase.ExecuteAsync(rentalId, cancellationToken);

            return rental is not null ? Ok(rental) : NoContent();
        }

        [HttpPost]
        [Produces(typeof(ResourceIdViewModel))]
        public async Task<ActionResult> Post([FromServices] IAddRentalUseCase addRentalUseCase,
            [FromBody] AddRentalInput input, CancellationToken cancellationToken)
        {
            try
            {
                var rental = await addRentalUseCase.ExecuteAsync(input, cancellationToken);
                var key = new ResourceIdViewModel { Id = rental.Id };
                return Ok(key);
            }
            catch (DataContractValidationException validationException)
            {
                return BadRequest(validationException.ValidationResult);
            }
        }
    }
}

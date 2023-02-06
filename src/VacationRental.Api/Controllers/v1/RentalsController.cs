using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading;
using System.Threading.Tasks;
using VacationRental.Api.Models;
using VacationRental.Application.Features.Rentals.AddRental.Domain;
using VacationRental.Application.Features.Rentals.AddRental.UseCase;
using VacationRental.Application.Features.Rentals.GetRental.UseCase;
using VacationRental.Application.Features.Rentals.UpdateRental.Domain;
using VacationRental.Application.Features.Rentals.UpdateRental.UseCase;
using VacationRental.Application.Shared.Domain.Exceptions;
using VacationRental.Application.Shared.Domain.Models;

namespace VacationRental.Api.Controllers.v1;

[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion("1.0")]
public class RentalsController : ControllerBase
{
    [HttpGet("{rentalId:int?}")]
    [ProducesResponseType(typeof(Rental), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> Get([FromServices] IGetRentalUseCase getRentalUseCase,
        [FromRoute] int? rentalId, CancellationToken cancellationToken)
    {
        var rental = await getRentalUseCase.ExecuteAsync(rentalId, cancellationToken);

        return rental is not null ? Ok(rental) : NoContent();
    }

    [HttpPost]
    [ProducesResponseType(typeof(ResourceIdViewModel), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Post([FromServices] IAddRentalUseCase addRentalUseCase,
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
            return BadRequest(validationException.ValidationErrorMessages);
        }
    }

    [HttpPut("{rentalId:int?}")]
    [ProducesResponseType(typeof(ResourceIdViewModel), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Post([FromServices] IUpdateRentalUseCase updateRentalUseCase,
        [FromRoute] int? rentalId, [FromBody] UpdateRentalBindingModel input, CancellationToken cancellationToken)
    {
        try
        {
            var useCaseInput = new UpdateRentalInput
            {
                RentalId = rentalId,
                Units = input.Units,
                PreparationTimeInDays = input.PreparationTimeInDays,
            };
            var rental = await updateRentalUseCase.ExecuteAsync(useCaseInput, cancellationToken);
            var key = new ResourceIdViewModel { Id = rental.Id };
            return Ok(key);
        }
        catch (RentalNotFoundException rentalNotFoundException)
        {
            return BadRequest(rentalNotFoundException.Message);
        }
        catch (DataContractValidationException validationException)
        {
            return BadRequest(validationException.ValidationErrorMessages);
        }
    }
}

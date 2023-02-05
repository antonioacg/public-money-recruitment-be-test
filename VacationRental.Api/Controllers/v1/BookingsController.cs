using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading;
using System.Threading.Tasks;
using VacationRental.Api.Models;
using VacationRental.Application.Features.Bookings.AddBooking.Domain;
using VacationRental.Application.Features.Bookings.AddBooking.UseCase;
using VacationRental.Application.Features.Bookings.GetBooking.UseCase;
using VacationRental.Application.Shared.Domain.Exceptions;
using VacationRental.Application.Shared.Domain.Models;

namespace VacationRental.Api.Controllers.v1;

[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion("1.0")]
public class BookingsController : ControllerBase
{
    [HttpGet("{bookingId:int?}")]
    [ProducesResponseType(typeof(Booking), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> Get([FromServices] IGetBookingUseCase getBookingUseCase,
        [FromRoute] int? bookingId, CancellationToken cancellationToken)
    {
        var booking = await getBookingUseCase.ExecuteAsync(bookingId, cancellationToken);

        return booking is not null ? Ok(booking) : NoContent();
    }

    [HttpPost]
    [ProducesResponseType(typeof(ResourceIdViewModel), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Post([FromServices] IAddBookingUseCase addBookingUseCase,
        [FromBody] AddBookingInput input, CancellationToken cancellationToken)
    {
        try
        {
            var booking = await addBookingUseCase.ExecuteAsync(input, cancellationToken);
            var key = new ResourceIdViewModel { Id = booking.Id };
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

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading;
using System.Threading.Tasks;
using VacationRental.Application.Features.Calendar.GetCalendar.Domain;
using VacationRental.Application.Features.Calendar.GetCalendar.Domain.Models;
using VacationRental.Application.Features.Calendar.GetCalendar.UseCase;
using VacationRental.Application.Shared.Domain.Exceptions;

namespace VacationRental.Api.Controllers.v1;

[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion("1.0")]
public class CalendarController : ControllerBase
{
    [HttpGet]
    [ProducesResponseType(typeof(Calendar), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Get([FromServices] IGetCalendarUseCase getCalendarUseCase, [FromQuery] int? rentalId,
        [FromQuery] DateTimeOffset? start, [FromQuery] int? nights, CancellationToken cancellationToken)
    {
        try
        {
            var getCalendarInput = new GetCalendarInput
            {
                RentalId = rentalId,
                Start = start,
                Nights = nights
            };
            var calendar = await getCalendarUseCase.ExecuteAsync(getCalendarInput, cancellationToken);
            return Ok(calendar);
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

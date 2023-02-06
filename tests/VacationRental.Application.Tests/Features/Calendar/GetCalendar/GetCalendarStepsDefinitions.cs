using FluentAssertions;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;
using VacationRental.Application.Features.Calendar.GetCalendar.UseCase;
using VacationRental.Application.Features.Calendar.GetCalendar.UseCase.Validator;
using VacationRental.Application.Shared.Domain.Models;
using VacationRental.Application.Shared.Repositories.Bookings;
using VacationRental.Application.Shared.Repositories.Rentals;
using VacationRental.Application.Tests.Extensions;
using VacationRental.Application.Tests.Features.Calendar.GetCalendar.Tables;
using VacationRental.Application.Tests.Shared.Tables;

namespace VacationRental.Application.Tests.Features.Calendar.GetCalendar;

[Binding]
[Scope(Feature = "GetCalendar")]
internal class GetCalendarStepsDefinitions
{
    private readonly ScenarioContext _scenarioContext;

    public GetCalendarStepsDefinitions(ScenarioContext scenarioContext)
    {
        _scenarioContext = scenarioContext;
    }

    [Given(@"that exists a rental with")]
    public void GivenThatExistsARentalWith(Table table)
    {
        var rental = table.CreateInstance<Rental>();
        _scenarioContext.GetMock<IRentalRepository>()
            .Setup(m => m.GetAsync(
                It.Is<int>(x => x == rental.Id),
                It.Is<CancellationToken>(x => x == _scenarioContext.Get<CancellationToken>())))
            .ReturnsAsync(rental);
    }

    [Given(@"that exists a booking with")]
    public void GivenThatExistsABookingWith(Table table)
    {
        var existingBookings = table.CreateSet<BookingTable>().Select(x => x.ToBooking());
        _scenarioContext.GetMock<IBookingRepository>()
            .Setup(m => m.GetByRentalAsync(
                It.IsAny<int>(),
                It.Is<CancellationToken>(x => x == _scenarioContext.Get<CancellationToken>())))
            .ReturnsAsync((int rentalId, CancellationToken _) => existingBookings.Where(b => b.RentalId == rentalId).ToList());
    }

    [When(@"fetching a calendar with")]
    public async Task WhenFetchingACalendarWith(Table table)
    {
        try
        {
            var getCalendarInput = table.CreateInstance<GetCalendarInputTable>().ToCalendarInput();
            var getCalendarUseCase = _scenarioContext.Get<GetCalendarUseCase>();
            var calendar = await getCalendarUseCase.ExecuteAsync(getCalendarInput, _scenarioContext.Get<CancellationToken>());
            _scenarioContext.Set(calendar);
        }
        catch (Exception ex)
        {
            _scenarioContext.Set(ex);
        }
    }

    [Then(@"should return calendar")]
    public void ThenShouldReturnCalendar(Table table)
    {
        _scenarioContext.TryGetValue(out Application.Features.Calendar.GetCalendar.Domain.Models.Calendar? actualCalendar).Should().BeTrue();
        var getCalendarOutputTables = table.CreateSet<GetCalendarOutputTable>();
        var expectedCalendar = GetCalendarOutputTable.ToCalendar(getCalendarOutputTables);
        actualCalendar.Should().BeEquivalentTo(expectedCalendar);
    }

    [Then(@"should throw '([^']*)'")]
    public void ThenShouldThrow(Exception expectedException)
    {
        _scenarioContext.TryGetValue(out Application.Features.Calendar.GetCalendar.Domain.Models.Calendar? _).Should().BeFalse();
        _scenarioContext.TryGetValue(out Exception actualException).Should().BeTrue();
        actualException.Should().BeOfType(expectedException.GetType());
    }

    [BeforeScenario]
    public static void BeforeScenario(ScenarioContext scenarioContext)
    {
        scenarioContext.Set(new CancellationTokenSource().Token);

        scenarioContext.SetMock<IBookingRepository>();
        scenarioContext.SetMock<IRentalRepository>();

        scenarioContext.Set(() => new GetCalendarUseCase(
            NullLogger<GetCalendarUseCase>.Instance,
            new GetCalendarInputValidator(),
            scenarioContext.Get<IBookingRepository>(),
            scenarioContext.Get<IRentalRepository>()));
    }
}

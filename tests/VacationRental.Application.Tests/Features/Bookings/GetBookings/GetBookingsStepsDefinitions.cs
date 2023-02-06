using System;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using TechTalk.SpecFlow;
using VacationRental.Application.Features.Bookings.GetBooking.UseCase;
using VacationRental.Application.Shared.Domain.Models;
using VacationRental.Application.Shared.Repositories.Bookings;
using VacationRental.Application.Tests.Extensions;

namespace VacationRental.Application.Tests.Features.Bookings.GetBookings;

[Binding]
[Scope(Feature = "GetBookings")]
internal class GetCalendarStepsDefinitions
{
    private readonly ScenarioContext _scenarioContext;

    public GetCalendarStepsDefinitions(ScenarioContext scenarioContext)
    {
        _scenarioContext = scenarioContext;
    }

    [Given(@"that exists a booking with id '([^']*)'")]
    public void GivenThatExistsABookingWithId(int id)
    {
        _scenarioContext.GetMock<IBookingRepository>()
            .Setup(m => m.GetAsync(
                It.Is<int>(x => x == id),
                It.Is<CancellationToken>(x => x == _scenarioContext.Get<CancellationToken>())))
            .ReturnsAsync(new Booking { Id = id });
    }

    [When(@"trying to fetch a booking with id '([^']*)'")]
    public async Task WhenTryingToFetchABookingWithId(int? id)
    {
        try
        {
            var getBookingUseCase = _scenarioContext.Get<GetBookingUseCase>();
            var booking = await getBookingUseCase.ExecuteAsync(id, _scenarioContext.Get<CancellationToken>());
            _scenarioContext.Set(booking);
        }
        catch (Exception ex)
        {
            _scenarioContext.Set(ex);
        }
    }

    [Then(@"should return the booking with id '([^']*)'")]
    public void ThenShouldReturnTheBookingWithId(int id)
    {
        _scenarioContext.TryGetValue(out Booking? booking).Should().BeTrue();
        booking!.Id.Should().Be(id);
    }

    [Then(@"should return null")]
    public void ThenShouldReturnNull()
    {
        _scenarioContext.TryGetValue(out Booking? booking).Should().BeTrue();
        booking.Should().BeNull();
    }

    [Then(@"should throw '([^']*)'")]
    public void ThenShouldThrow(Exception expectedException)
    {
        _scenarioContext.TryGetValue(out Booking? _).Should().BeFalse();
        _scenarioContext.TryGetValue(out Exception actualException).Should().BeTrue();
        actualException.Should().BeOfType(expectedException.GetType());
    }

    [BeforeScenario]
    public static void BeforeScenario(ScenarioContext scenarioContext)
    {
        scenarioContext.Set(new CancellationTokenSource().Token);

        scenarioContext.SetMock<IBookingRepository>();

        scenarioContext.Set(() => new GetBookingUseCase(
            NullLogger<GetBookingUseCase>.Instance,
            scenarioContext.Get<IBookingRepository>()));
    }
}

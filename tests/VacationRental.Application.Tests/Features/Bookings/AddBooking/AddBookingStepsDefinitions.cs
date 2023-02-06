using FluentAssertions;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;
using VacationRental.Application.Features.Bookings.AddBooking.UseCase;
using VacationRental.Application.Features.Bookings.AddBooking.UseCase.Validator;
using VacationRental.Application.Shared.Domain.Models;
using VacationRental.Application.Shared.Repositories.Bookings;
using VacationRental.Application.Shared.Repositories.Rentals;
using VacationRental.Application.Tests.Extensions;
using VacationRental.Application.Tests.Features.Bookings.AddBooking.Tables;
using VacationRental.Application.Tests.Shared.Tables;

namespace VacationRental.Application.Tests.Features.Bookings.AddBooking;

[Binding]
[Scope(Feature = "AddBookings")]
internal class AddBookingStepsDefinitions
{
    private readonly ScenarioContext _scenarioContext;

    public AddBookingStepsDefinitions(ScenarioContext scenarioContext)
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
        _scenarioContext.Set(existingBookings.Select(x => x.Id).DefaultIfEmpty(0).Max() + 1, "nextBookingId");
        _scenarioContext.GetMock<IBookingRepository>()
            .Setup(m => m.GetByRentalAsync(
                It.IsAny<int>(),
                It.Is<CancellationToken>(x => x == _scenarioContext.Get<CancellationToken>())))
            .ReturnsAsync((int rentalId, CancellationToken _) => existingBookings.Where(b => b.RentalId == rentalId).ToList());
    }

    [When(@"trying to add a booking with")]
    public async Task WhenTryingToAddABookingWith(Table table)
    {
        try
        {
            var addBookingInput = table.CreateInstance<AddBookingInputTable>().ToAddBookingInput();
            var addBookingUseCase = _scenarioContext.Get<AddBookingUseCase>();
            var booking = await addBookingUseCase.ExecuteAsync(addBookingInput, _scenarioContext.Get<CancellationToken>());
            _scenarioContext.Set(booking);
        }
        catch (Exception ex)
        {
            _scenarioContext.Set(ex);
        }
    }

    [Then(@"should return the booking with")]
    public void ThenShouldReturnTheBookingWith(Table table)
    {
        _scenarioContext.TryGetValue(out Booking? actualBooking).Should().BeTrue();
        var expectedBooking = table.CreateInstance<BookingTable>().ToBooking();
        actualBooking.Should().BeEquivalentTo(expectedBooking);
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

        scenarioContext.Set(1, "nextBookingId");
        scenarioContext.SetMock<IBookingRepository>()
            .Setup(m => m.AddAsync(
                It.IsAny<Booking>(),
                It.Is<CancellationToken>(x => x == scenarioContext.Get<CancellationToken>())))
            .Callback((Booking booking, CancellationToken _) =>
            {
                booking.Id = scenarioContext.Get<int>("nextBookingId");
                scenarioContext.Set(booking.Id + 1, "nextBookingId");
                scenarioContext.Set(booking);
            });

        scenarioContext.SetMock<IRentalRepository>();

        scenarioContext.Set(() => new AddBookingUseCase(
            NullLogger<AddBookingUseCase>.Instance,
            new AddBookingInputValidator(),
            scenarioContext.Get<IBookingRepository>(),
            scenarioContext.Get<IRentalRepository>()));
    }
}

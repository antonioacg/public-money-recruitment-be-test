using FluentAssertions;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;
using VacationRental.Application.Features.Rentals.UpdateRental.UseCase;
using VacationRental.Application.Features.Rentals.UpdateRental.UseCase.Validator;
using VacationRental.Application.Shared.Domain.Models;
using VacationRental.Application.Shared.Repositories.Bookings;
using VacationRental.Application.Shared.Repositories.Rentals;
using VacationRental.Application.Tests.Extensions;
using VacationRental.Application.Tests.Features.Rentals.UpdateRentals.Tables;
using VacationRental.Application.Tests.Shared.Tables;

namespace VacationRental.Application.Tests.Features.Rentals.UpdateRentals;

[Binding]
[Scope(Feature = "UpdateRental")]
internal class UpdateRentalStepsDefinitions
{
    private readonly ScenarioContext _scenarioContext;

    public UpdateRentalStepsDefinitions(ScenarioContext scenarioContext)
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

    [When(@"trying to update a rental with")]
    public async Task WhenTryingToUpdateARentalWith(Table table)
    {
        try
        {
            var updateRentalInput = table.CreateInstance<UpdateRentalInputTable>().ToUpdateRentalInput();
            var updateRentalUseCase = _scenarioContext.Get<UpdateRentalUseCase>();
            var rental = await updateRentalUseCase.ExecuteAsync(updateRentalInput, _scenarioContext.Get<CancellationToken>());
            _scenarioContext.Set(rental);
        }
        catch (Exception ex)
        {
            _scenarioContext.Set(ex);
        }
    }

    [Then(@"should return the rental with")]
    public void ThenShouldReturnTheRentalWith(Table table)
    {
        _scenarioContext.TryGetValue(out Rental? actualRental).Should().BeTrue();
        var expectedRental = table.CreateInstance<Rental>();
        actualRental.Should().BeEquivalentTo(expectedRental);
    }

    [Then(@"should throw '([^']*)'")]
    public void ThenShouldThrow(Exception expectedException)
    {
        _scenarioContext.TryGetValue(out Rental? _).Should().BeFalse();
        _scenarioContext.TryGetValue(out Exception actualException).Should().BeTrue();
        actualException.Should().BeOfType(expectedException.GetType());
    }

    [BeforeScenario]
    public static void BeforeScenario(ScenarioContext scenarioContext)
    {
        scenarioContext.Set(new CancellationTokenSource().Token);

        scenarioContext.SetMock<IBookingRepository>();
        scenarioContext.SetMock<IRentalRepository>()
            .Setup(m => m.UpdateAsync(
                It.IsAny<Rental>(),
                It.Is<CancellationToken>(x => x == scenarioContext.Get<CancellationToken>())));

        scenarioContext.Set(() => new UpdateRentalUseCase(
            NullLogger<UpdateRentalUseCase>.Instance,
            new UpdateRentalInputValidator(),
            scenarioContext.Get<IRentalRepository>(),
            scenarioContext.Get<IBookingRepository>()));
    }
}

using FluentAssertions;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using System;
using System.Threading;
using System.Threading.Tasks;
using TechTalk.SpecFlow;
using VacationRental.Application.Features.Rentals.GetRental.UseCase;
using VacationRental.Application.Shared.Domain.Models;
using VacationRental.Application.Shared.Repositories.Rentals;
using VacationRental.Application.Tests.Extensions;

namespace VacationRental.Application.Tests.Features.Rentals.GetRentals;

[Binding]
[Scope(Feature = "GetRentals")]
internal class GetRentalsStepsDefinitions
{
    private readonly ScenarioContext _scenarioContext;

    public GetRentalsStepsDefinitions(ScenarioContext scenarioContext)
    {
        _scenarioContext = scenarioContext;
    }

    [Given(@"that exists a rental with id '([^']*)'")]
    public void GivenThatExistsARentalWithId(int id)
    {
        _scenarioContext.GetMock<IRentalRepository>()
            .Setup(m => m.GetAsync(
                It.Is<int>(x => x == id),
                It.Is<CancellationToken>(x => x == _scenarioContext.Get<CancellationToken>())))
            .ReturnsAsync(new Rental { Id = id });
    }

    [When(@"trying to fetch a rental with id '([^']*)'")]
    public async Task WhenTryingToFetchARentalWithId(int? id)
    {
        try
        {
            var getRentalUseCase = _scenarioContext.Get<GetRentalUseCase>();
            var rental = await getRentalUseCase.ExecuteAsync(id, _scenarioContext.Get<CancellationToken>());
            _scenarioContext.Set(rental);
        }
        catch (Exception ex)
        {
            _scenarioContext.Set(ex);
        }
    }

    [Then(@"should return the rental with id '([^']*)'")]
    public void ThenShouldReturnTheRentalWithId(int id)
    {
        _scenarioContext.TryGetValue(out Rental? rental).Should().BeTrue();
        rental!.Id.Should().Be(id);
    }

    [Then(@"should return null")]
    public void ThenShouldReturnNull()
    {
        _scenarioContext.TryGetValue(out Rental? rental).Should().BeTrue();
        rental.Should().BeNull();
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

        scenarioContext.SetMock<IRentalRepository>();

        scenarioContext.Set(() => new GetRentalUseCase(
            NullLogger<GetRentalUseCase>.Instance,
            scenarioContext.Get<IRentalRepository>()));
    }
}

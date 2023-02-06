using FluentAssertions;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using System;
using System.Threading;
using System.Threading.Tasks;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;
using VacationRental.Application.Features.Rentals.AddRental.UseCase;
using VacationRental.Application.Features.Rentals.AddRental.UseCase.Validator;
using VacationRental.Application.Shared.Domain.Models;
using VacationRental.Application.Shared.Repositories.Rentals;
using VacationRental.Application.Tests.Extensions;
using VacationRental.Application.Tests.Features.Rentals.AddRental.Tables;

namespace VacationRental.Application.Tests.Features.Rentals.AddRental;

[Binding]
[Scope(Feature = "AddRentals")]
internal class AddRentalStepsDefinitions
{
    private readonly ScenarioContext _scenarioContext;

    public AddRentalStepsDefinitions(ScenarioContext scenarioContext)
    {
        _scenarioContext = scenarioContext;
    }

    [When(@"trying to add a rental with")]
    public async Task WhenTryingToAddARentalWith(Table table)
    {
        try
        {
            var addRentalInput = table.CreateInstance<AddRentalInputTable>().ToAddRentalInput();
            var addRentalUseCase = _scenarioContext.Get<AddRentalUseCase>();
            var rental = await addRentalUseCase.ExecuteAsync(addRentalInput, _scenarioContext.Get<CancellationToken>());
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

        var id = 1;
        scenarioContext.SetMock<IRentalRepository>()
            .Setup(m => m.AddAsync(
                It.IsAny<Rental>(),
                It.Is<CancellationToken>(x => x == scenarioContext.Get<CancellationToken>())))
            .Callback((Rental rental, CancellationToken _) =>
            {
                rental.Id = id;
                id++;
                scenarioContext.Set(rental);
            });

        scenarioContext.Set(() => new AddRentalUseCase(
            NullLogger<AddRentalUseCase>.Instance,
            new AddRentalInputValidator(),
            scenarioContext.Get<IRentalRepository>()));
    }
}

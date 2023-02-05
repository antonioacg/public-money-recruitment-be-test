using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using VacationRental.Api.Models;
using VacationRental.Application.Features.Rentals.AddRental.Domain;
using VacationRental.Application.Shared.Domain.Models;
using Xunit;

namespace VacationRental.Api.Tests;

[Collection("Integration")]
public class PostRentalTests
{
    private readonly HttpClient _client;

    public PostRentalTests(IntegrationFixture fixture)
    {
        _client = fixture.Client;
    }

    [Fact]
    public async Task GivenCompleteRequest_WhenPostRental_ThenAGetReturnsTheCreatedRental()
    {
        var request = new AddRentalInput
        {
            Units = 25
        };

        ResourceIdViewModel? postResult;
        using (var postResponse = await _client.PostAsJsonAsync("/api/v1/rentals", request))
        {
            Assert.True(postResponse.IsSuccessStatusCode);
            postResult = await postResponse.Content.ReadFromJsonAsync<ResourceIdViewModel>();
            Assert.NotNull(postResult);
        }

        using (var getResponse = await _client.GetAsync($"/api/v1/rentals/{postResult.Id}"))
        {
            Assert.True(getResponse.IsSuccessStatusCode);

            var getResult = await getResponse.Content.ReadFromJsonAsync<Rental>();
            Assert.NotNull(getResult);
            Assert.Equal(request.Units, getResult.Units);
        }
    }
}

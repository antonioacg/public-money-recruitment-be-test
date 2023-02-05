using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using VacationRental.Api.Models;
using VacationRental.Application.Features.Bookings.AddBooking.Domain;
using VacationRental.Application.Features.Rentals.AddRental.Domain;
using VacationRental.Application.Shared.Domain.Models;
using Xunit;

namespace VacationRental.Api.Tests;

[Collection("Integration")]
public class PostBookingTests
{
    private readonly HttpClient _client;

    public PostBookingTests(IntegrationFixture fixture)
    {
        _client = fixture.Client;
    }

    [Fact]
    public async Task GivenCompleteRequest_WhenPostBooking_ThenAGetReturnsTheCreatedBooking()
    {
        var postRentalRequest = new AddRentalInput
        {
            Units = 4
        };

        ResourceIdViewModel? postRentalResult;
        using (var postRentalResponse = await _client.PostAsJsonAsync("/api/v1/rentals", postRentalRequest))
        {
            Assert.True(postRentalResponse.IsSuccessStatusCode);
            postRentalResult = await postRentalResponse.Content.ReadFromJsonAsync<ResourceIdViewModel>();
            Assert.NotNull(postRentalResult);
        }

        var postBookingRequest = new AddBookingInput
        {
            RentalId = postRentalResult.Id,
            Nights = 3,
            Start = DateTime.Now.AddDays(1)
        };

        ResourceIdViewModel? postBookingResult;
        using (var postBookingResponse = await _client.PostAsJsonAsync("/api/v1/bookings", postBookingRequest))
        {
            Assert.True(postBookingResponse.IsSuccessStatusCode);
            postBookingResult = await postBookingResponse.Content.ReadFromJsonAsync<ResourceIdViewModel>();
            Assert.NotNull(postBookingResult);
        }

        using (var getBookingResponse = await _client.GetAsync($"/api/v1/bookings/{postBookingResult.Id}"))
        {
            Assert.True(getBookingResponse.IsSuccessStatusCode);

            var getBookingResult = await getBookingResponse.Content.ReadFromJsonAsync<Booking>();
            Assert.NotNull(getBookingResult);
            Assert.Equal(postBookingRequest.RentalId, getBookingResult.RentalId);
            Assert.Equal(postBookingRequest.Nights, getBookingResult.Nights);
            Assert.Equal(postBookingRequest.Start!.Value.Date, getBookingResult.Start);
        }
    }

    [Fact]
    public async Task GivenCompleteRequest_WhenPostBooking_ThenAPostReturnsErrorWhenThereIsOverbooking()
    {
        var postRentalRequest = new AddRentalInput
        {
            Units = 1
        };

        ResourceIdViewModel? postRentalResult;
        using (var postRentalResponse = await _client.PostAsJsonAsync("/api/v1/rentals", postRentalRequest))
        {
            Assert.True(postRentalResponse.IsSuccessStatusCode);
            postRentalResult = await postRentalResponse.Content.ReadFromJsonAsync<ResourceIdViewModel>();
            Assert.NotNull(postRentalResult);
        }

        var postBooking1Request = new AddBookingInput
        {
            RentalId = postRentalResult.Id,
            Nights = 3,
            Start = DateTime.Now.AddDays(1)
        };

        using (var postBooking1Response = await _client.PostAsJsonAsync("/api/v1/bookings", postBooking1Request))
        {
            Assert.True(postBooking1Response.IsSuccessStatusCode);
        }

        var postBooking2Request = new AddBookingInput
        {
            RentalId = postRentalResult.Id,
            Nights = 1,
            Start = DateTime.Now.AddDays(2)
        };

        using (var postBooking2Response = await _client.PostAsJsonAsync("/api/v1/bookings", postBooking2Request))
        {
            Assert.True(postBooking2Response.StatusCode is HttpStatusCode.InternalServerError);
        }
    }
}

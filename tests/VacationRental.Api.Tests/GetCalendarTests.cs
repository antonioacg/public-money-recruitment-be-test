using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using VacationRental.Api.Models;
using VacationRental.Application.Features.Bookings.AddBooking.Domain;
using VacationRental.Application.Features.Calendar.GetCalendar.Domain.Models;
using VacationRental.Application.Features.Rentals.AddRental.Domain;
using Xunit;

namespace VacationRental.Api.Tests;

[Collection("Integration")]
public class GetCalendarTests
{
    private readonly HttpClient _client;

    public GetCalendarTests(IntegrationFixture fixture)
    {
        _client = fixture.Client;
    }

    [Fact]
    public async Task GivenCompleteRequest_WhenGetCalendar_ThenAGetReturnsTheCalculatedCalendar()
    {
        var postRentalRequest = new AddRentalInput
        {
            Units = 2
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
            Nights = 2,
            Start = DateTime.Now.AddDays(2)
        };

        ResourceIdViewModel? postBooking1Result;
        using (var postBooking1Response = await _client.PostAsJsonAsync("/api/v1/bookings", postBooking1Request))
        {
            Assert.True(postBooking1Response.IsSuccessStatusCode);
            postBooking1Result = await postBooking1Response.Content.ReadFromJsonAsync<ResourceIdViewModel>();
            Assert.NotNull(postBooking1Result);
        }

        var postBooking2Request = new AddBookingInput
        {
            RentalId = postRentalResult.Id,
            Nights = 2,
            Start = DateTime.Now.AddDays(3)
        };

        ResourceIdViewModel? postBooking2Result;
        using (var postBooking2Response = await _client.PostAsJsonAsync("/api/v1/bookings", postBooking2Request))
        {
            Assert.True(postBooking2Response.IsSuccessStatusCode);
            postBooking2Result = await postBooking2Response.Content.ReadFromJsonAsync<ResourceIdViewModel>();
            Assert.NotNull(postBooking2Result);
        }

        var start = DateTime.Now.AddDays(1).Date.ToString("O");
        using (var getCalendarResponse = await _client.GetAsync($"/api/v1/calendar?rentalId={postRentalResult.Id}&start={start}&nights=5"))
        {
            Assert.True(getCalendarResponse.IsSuccessStatusCode);

            var getCalendarResult = await getCalendarResponse.Content.ReadFromJsonAsync<Calendar>();
            Assert.NotNull(getCalendarResult);

            Assert.Equal(postRentalResult.Id, getCalendarResult.RentalId);
            Assert.Equal(5, getCalendarResult.Dates.Count);

            Assert.Equal(DateTime.Now.AddDays(1).Date, getCalendarResult.Dates[0].Date);
            Assert.Empty(getCalendarResult.Dates[0].Bookings);

            Assert.Equal(DateTime.Now.AddDays(2).Date, getCalendarResult.Dates[1].Date);
            Assert.Single(getCalendarResult.Dates[1].Bookings);
            Assert.Contains(getCalendarResult.Dates[1].Bookings, x => x.Id == postBooking1Result.Id);

            Assert.Equal(DateTime.Now.AddDays(3).Date, getCalendarResult.Dates[2].Date);
            Assert.Equal(2, getCalendarResult.Dates[2].Bookings.Count);
            Assert.Contains(getCalendarResult.Dates[2].Bookings, x => x.Id == postBooking1Result.Id);
            Assert.Contains(getCalendarResult.Dates[2].Bookings, x => x.Id == postBooking2Result.Id);

            Assert.Equal(DateTime.Now.AddDays(4).Date, getCalendarResult.Dates[3].Date);
            Assert.Single(getCalendarResult.Dates[3].Bookings);
            Assert.Contains(getCalendarResult.Dates[3].Bookings, x => x.Id == postBooking2Result.Id);

            Assert.Equal(DateTime.Now.AddDays(5).Date, getCalendarResult.Dates[4].Date);
            Assert.Empty(getCalendarResult.Dates[4].Bookings);
        }
    }
}

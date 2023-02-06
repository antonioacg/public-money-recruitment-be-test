using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using System;
using System.Net.Http;
using Xunit;

namespace VacationRental.Api.Tests;

[CollectionDefinition("Integration")]
public sealed class IntegrationFixture : IDisposable, ICollectionFixture<IntegrationFixture>
{
    private readonly TestServer _server;

    public HttpClient Client { get; }

    public IntegrationFixture()
    {
        var application = new WebApplicationFactory<Program>();

        _server = application.Server;

        Client = application.CreateClient();
    }

    public void Dispose()
    {
        Client.Dispose();
        _server.Dispose();
    }
}

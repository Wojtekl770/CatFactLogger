using System.Net;
using System.Net.Http.Json;
using CatFactLogger.Repositories.Interfaces;
using CatFactLogger.Services;
using CatFactLogger.Shared.Data;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using Moq;
using Xunit;

namespace CatFactLogger.Tests;

public class FactServiceTests
{
    private static HttpClient CreateHttpClient(HttpStatusCode statusCode, object? body)
    {
        var handler = new FakeHttpMessageHandler(_ => new HttpResponseMessage(statusCode)
        {
            Content = body is null ? null : JsonContent.Create(body)
        });

        return new HttpClient(handler) { BaseAddress = new Uri("https://catfact.ninja/") };
    }

    [Fact]
    public async Task FetchAndSaveAsync_ReturnsFactAndSavesIt_WhenApiReturnsData()
    {
        var fact = new CatFact("Cats sleep a lot.", 18);
        var httpClient = CreateHttpClient(HttpStatusCode.OK, fact);

        var repositoryMock = new Mock<IFactRepository>();
        var loggerMock = new Mock<ILogger<FactService>>();

        var sut = new FactService(httpClient, repositoryMock.Object, loggerMock.Object);

        var result = await sut.FetchAndSaveAsync();

        Assert.Equal(fact, result);
        repositoryMock.Verify(r => r.AppendAsync(
            It.Is<CatFact>(f => f.Fact == fact.Fact && f.Length == fact.Length),
            It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task FetchAndSaveAsync_Throws_WhenApiReturnsEmptyBody()
    {
        var httpClient = CreateHttpClient(HttpStatusCode.OK, null);

        var repositoryMock = new Mock<IFactRepository>();
        var loggerMock = new Mock<ILogger<FactService>>();

        var sut = new FactService(httpClient, repositoryMock.Object, loggerMock.Object);

        await Assert.ThrowsAsync<JsonException>(
            () => sut.FetchAndSaveAsync());
        repositoryMock.Verify(r => r.AppendAsync(It.IsAny<CatFact>(), It.IsAny<CancellationToken>()), Times.Never);
    }
}
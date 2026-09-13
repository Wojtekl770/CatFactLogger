using System.Net;
using System.Net.Http.Json;
using CatFactLogger.Repositories.Interfaces;
using CatFactLogger.Services;
using CatFactLogger.Services.Interfaces;
using CatFactLogger.Shared.Data;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace CatFactLogger.Tests;

public class FactControllerTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;

    public FactControllerTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureServices(services =>
            {
                services.AddHttpClient<IFactService, FactService>(client =>
                {
                    client.BaseAddress = new Uri("https://catfact.ninja/");
                })
                .ConfigurePrimaryHttpMessageHandler(() => new FakeHttpMessageHandler(_ =>
                    new HttpResponseMessage(HttpStatusCode.OK)
                    {
                        Content = JsonContent.Create(new CatFact("Fake fact for tests.", 20))
                    }));

                var repositoryDescriptor = services.Single(d => d.ServiceType == typeof(IFactRepository));
                services.Remove(repositoryDescriptor);
                services.AddSingleton<IFactRepository, NoOpFactRepository>();
            });
        });
    }

    [Fact]
    public async Task PostFact_ReturnsOkWithFact()
    {
        var client = _factory.CreateClient();

        var response = await client.PostAsync("/api/Fact", null);

        response.EnsureSuccessStatusCode();
        var fact = await response.Content.ReadFromJsonAsync<CatFact>();
        Assert.NotNull(fact);
        Assert.Equal("Fake fact for tests.", fact!.Fact);
    }

    private class NoOpFactRepository : IFactRepository
    {
        public Task AppendAsync(CatFact fact, CancellationToken ct = default) => Task.CompletedTask;
    }
}
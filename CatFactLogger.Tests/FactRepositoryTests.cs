using CatFactLogger.Repositories;
using CatFactLogger.Shared.Data;
using Microsoft.Extensions.Logging;

namespace CatFactLogger.Tests;
public class FactRepositoryTests : IDisposable
{
    private readonly string _tempFilePath =
        Path.Combine(Path.GetTempPath(), $"facts_test_{Guid.NewGuid()}.txt");

    private readonly ILogger<FactRepository> _logger;

    public FactRepositoryTests()
    {
        var loggerFactory = LoggerFactory.Create(builder => { });
        _logger = loggerFactory.CreateLogger<FactRepository>();
    }

    [Fact]
    public async Task AppendAsync_CreatesFileAndWritesLine_WhenFileDoesNotExist()
    {
        var sut = new FactRepository(_tempFilePath, _logger);
        var fact = new CatFact("Test fact.", 10);

        await sut.AppendAsync(fact);

        Assert.True(File.Exists(_tempFilePath));

        var content = await File.ReadAllTextAsync(_tempFilePath);

        Assert.Contains("Test fact.", content);
        Assert.Contains("length=10", content);
    }

    [Fact]
    public async Task AppendAsync_AppendsNewLine_WhenCalledMultipleTimes()
    {
        var sut = new FactRepository(_tempFilePath, _logger);

        await sut.AppendAsync(new CatFact("First.", 6));
        await sut.AppendAsync(new CatFact("Second.", 7));

        var lines = await File.ReadAllLinesAsync(_tempFilePath);

        Assert.Equal(2, lines.Length);
    }

    public void Dispose()
    {
        if (File.Exists(_tempFilePath))
            File.Delete(_tempFilePath);
    }
}
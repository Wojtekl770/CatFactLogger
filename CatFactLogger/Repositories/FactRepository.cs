using CatFactLogger.Repositories.Interfaces;
using CatFactLogger.Shared.Data;

namespace CatFactLogger.Repositories
{
    public sealed class FactRepository : IFactRepository
    {
        private const string _filePath = "facts.txt";

        private readonly ILogger<FactRepository> _logger;
        private readonly SemaphoreSlim _writeLock = new(1, 1);

        public FactRepository(ILogger<FactRepository> logger)
        {
            _logger = logger;
        }

        public async Task AppendAsync(CatFact fact, CancellationToken ct = default)
        {
            var line = $"{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss} | {fact.Fact} | length={fact.Length}";
            await _writeLock.WaitAsync(ct);
            try
            {
                await File.AppendAllTextAsync(_filePath, line + Environment.NewLine, ct);
            }
            finally
            {
                _writeLock.Release();
            }
            _logger.LogInformation("Zapisano fakt do pliku: {Fact}", fact.Fact);
        }
    }
}

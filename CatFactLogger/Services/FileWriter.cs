using CatFactLogger.Shared.Data;
using CatFactLogger.Services.Interfaces;

namespace CatFactLogger.Services
{
    public sealed class FileWriter : IFileWriter
    {
        private readonly string _filePath;
        public FileWriter(string filePath)
        {
            _filePath = filePath;
        }
        public Task AppendAsync(CatFact fact, CancellationToken ct = default)
        {
            var line = $"{DateTime.UtcNow:O} | {fact.Fact} | length={fact.Length}";
            return File.AppendAllTextAsync(_filePath, line + Environment.NewLine, ct);
        }
    }
}

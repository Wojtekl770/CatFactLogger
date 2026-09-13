using CatFactLogger.Shared.Data;

namespace CatFactLogger.Services.Interfaces
{
    public interface IFileWriter
    {
        Task AppendAsync(CatFact fact, CancellationToken ct = default);
    }
}

using CatFactLogger.Shared.Data;

namespace CatFactLogger.Repositories.Interfaces
{
    public interface IFactRepository
    {
        Task AppendAsync(CatFact fact, CancellationToken ct = default);
    }
}

using CatFactLogger.Shared.Data;

namespace CatFactLogger.Services.Interfaces
{
    public interface IFactService
    {
        Task<CatFact> FetchAndSaveAsync(CancellationToken ct = default);
    }
}

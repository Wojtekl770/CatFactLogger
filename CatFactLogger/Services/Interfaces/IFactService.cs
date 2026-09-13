using CatFactLogger.Shared.Data;

namespace CatFactLogger.Services.Interfaces
{
    public interface IFactService
    {
        Task<CatFact?> GetRandomFactAsync(CancellationToken ct = default);
    }
}

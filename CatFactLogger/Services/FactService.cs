using CatFactLogger.Services.Interfaces;
using CatFactLogger.Shared.Data;

namespace CatFactLogger.Services
{
    public sealed class FactService : IFactService
    {
        private readonly HttpClient _httpClient;
        public FactService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public Task<CatFact?> GetRandomFactAsync(CancellationToken ct = default)
        {
            return _httpClient.GetFromJsonAsync<CatFact>("fact", ct);
        }
    }
}

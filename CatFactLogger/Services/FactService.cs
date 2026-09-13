using CatFactLogger.Repositories.Interfaces;
using CatFactLogger.Services.Interfaces;
using CatFactLogger.Shared.Data;

namespace CatFactLogger.Services
{
    public sealed class FactService : IFactService
    {

        private readonly HttpClient _httpClient;
        private readonly IFactRepository _repository;
        private readonly ILogger<FactService> _logger;
        public FactService(HttpClient httpClient, IFactRepository repository, ILogger<FactService> logger)
        {
            _httpClient = httpClient;
            _repository = repository;
            _logger = logger;
        }
        public async Task<CatFact> FetchAndSaveAsync(CancellationToken ct = default)
        {
            _logger.LogInformation("Pobieram nowy fakt z catfact.ninja...");
            var fact = await _httpClient.GetFromJsonAsync<CatFact>("fact", ct)
                ?? throw new InvalidOperationException("API zwróciło pustą odpowiedź.");

            await _repository.AppendAsync(fact, ct);
            return fact;
        }
    }
}

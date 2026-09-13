using CatFactLogger.Services.Interfaces;
using CatFactLogger.Shared.Data;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace CatFactLogger.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FactController: ControllerBase
    {
        private readonly IFactService _service;
        private readonly ILogger<FactController> _logger;

        public FactController(IFactService service, ILogger<FactController> logger)
        {
            _service = service;
            _logger = logger;
        }

        [HttpPost]
        public async Task<ActionResult<CatFact>> Create(CancellationToken ct)
        {
            try
            {
                var fact = await _service.FetchAndSaveAsync(ct);
                return Ok(fact);
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "Błąd podczas pobierania faktu z catfact.ninja.");
                return StatusCode(StatusCodes.Status502BadGateway,
                    "Nie udało się pobrać danych z zewnętrznego API.");
            }
            catch (TaskCanceledException ex) when (!ct.IsCancellationRequested)
            {
                _logger.LogError(ex, "Przekroczono czas oczekiwania na catfact.ninja.");
                return StatusCode(StatusCodes.Status504GatewayTimeout,
                    "Zewnętrzne API nie odpowiedziało w oczekiwanym czasie.");
            }
            catch (JsonException ex)
            {
                _logger.LogError(ex, "Nie udało się sparsować odpowiedzi z catfact.ninja.");
                return StatusCode(StatusCodes.Status502BadGateway,
                    "Otrzymano nieprawidłową odpowiedź z zewnętrznego API.");
            }
            catch (IOException ex)
            {
                _logger.LogError(ex, "Błąd zapisu do pliku z faktami.");
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Nie udało się zapisać danych.");
            }
        }
    }
}

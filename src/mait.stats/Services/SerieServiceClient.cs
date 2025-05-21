using MAIT.Dto.Serie;
using MAIT.Interfaces;

namespace mait.stats.Services;


public class SerieSeriveClient(HttpClient http, ILogger<SerieSeriveClient> logger)
{
    private readonly HttpClient _http = http;
    private readonly ILogger<SerieSeriveClient> _logger = logger;

    public async Task<int> CountAsync()
    {
        try
        {
            var response = await _http.GetAsync("/series/count");
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadFromJsonAsync<ResultModel<int>>();
            return result.Data;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener el conteo de series");
            return -1;
        }
    }
    
    public async Task<ICollection<SerieDto>?> GetAllAsync()
    {
        try
        {
            var response = await _http.GetAsync($"/series");
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadFromJsonAsync<ResultModel<SerieDto[]>>();
            return result.Data;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener las series");
            return [];
        }
    }
}

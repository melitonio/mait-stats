using MAIT.Dto.Documents;
using MAIT.Interfaces;

namespace Services;


public class DocumentServiceClient(HttpClient http, ILogger<DocumentServiceClient> logger)
{
    private readonly HttpClient _http = http;
    private readonly ILogger<DocumentServiceClient> _logger = logger;

    public async Task<int> CountAsync()
    {
        try
        {
            var response = await _http.GetAsync("/documents/count");
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadFromJsonAsync<ResultModel<int>>();
            return result.Data;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener el conteo de documentos");
            return -1;
        }
    }
    
    public async Task<ICollection<DocumentDto>?> GetAllAsync()
    {
        try
        {
            var response = await _http.GetAsync($"/documents");
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadFromJsonAsync<ResultModel<DocumentDto[]>>();
            return result.Data;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener los documentos");
            return [];
        }
    }
}

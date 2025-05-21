using MAIT.Dto.Funcionario;
using MAIT.Dto.Serie;
using MAIT.Interfaces;

namespace mait.stats.Services;


public class FuncionarioServiceClient(HttpClient http, ILogger<FuncionarioServiceClient> logger)
{
    private readonly HttpClient _http = http;
    private readonly ILogger<FuncionarioServiceClient> _logger = logger;

    public async Task<int> CountAsync()
    {
        try
        {
            var response = await _http.GetAsync("/funcionarios/count");
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadFromJsonAsync<ResultModel<int>>();
            return result.Data;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener el conteo de funcionarios");
            return -1;
        }
    }
    
    public async Task<ICollection<FuncionarioDto>?> GetAllAsync()
    {
        try
        {
            var response = await _http.GetAsync($"/funcionarios");
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadFromJsonAsync<ResultModel<FuncionarioDto[]>>();
            return result.Data;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener los funcionarios");
            return [];
        }
    }
}

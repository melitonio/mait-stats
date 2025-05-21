using Microsoft.AspNetCore.Mvc;

namespace mait.stats.Services;

[Route("[controller]/[action]")]
public class StatsController
(
    SerieSeriveClient series,
    FuncionarioServiceClient funcionarios,
    DocumentServiceClient documents
) : Controller
{
    private readonly SerieSeriveClient _series = series;
    private readonly FuncionarioServiceClient _funcionarios = funcionarios;
    private readonly DocumentServiceClient _documentos = documents;


    [HttpGet]
    public async Task<IActionResult> Dashboard()
    {
        var seriesCount = await _series.GetAllAsync();
        var funcionariosCount = await _funcionarios.GetAllAsync();
      //  var documentosCount = await _documentos.GetAllAsync();

        return Ok(new
        {
            Series = seriesCount?.Count,
            Funcionarios = funcionariosCount?.Count,
            Documentos = seriesCount?.Sum(x => x.Total),
        });
    }

}

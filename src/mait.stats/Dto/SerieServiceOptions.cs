namespace Mait.Stats.Dto;

public record ClientsOptions
{
    public string Series { get; set; } = string.Empty;
    public string Documents { get; set; } = string.Empty;
    public string Funcionario { get; set; } = string.Empty;
}
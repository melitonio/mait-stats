namespace mait.stats.Dto;

public record class StatsDashboardDto
{
    public int Users { get; init; } = 0;
    public int Series { get; init; } = 0;
    public int Documents { get; init; } = 0;
    public int Contacts { get; init; } = 0;
    public Dictionary<string, int> History { get; init; } = [];
}

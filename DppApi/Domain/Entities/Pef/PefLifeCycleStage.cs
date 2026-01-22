namespace DppApi.Domain.Entities.Pef;

public class PefLifeCycleStage
{
    public string Name { get; set; } = default!;
    public List<PefEmission> Emissions { get; set; } = [];
}

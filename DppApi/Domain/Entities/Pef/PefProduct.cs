namespace DppApi.Domain.Entities.Pef;

public class PefProduct
{
    public string Method { get; set; } = default!;
    public string ImpactCategory { get; set; } = default!;
    public PefFunctionalUnit FunctionalUnit { get; set; } = new();
    public List<PefLifeCycleStage> LifeCycleStages { get; set; } = [];
}

namespace DppApi.Domain.Entities.Pef;

public class PefEmission
{
    public string Substance { get; set; } = default!;
    public double Value { get; set; }
    public string Unit { get; set; } = default!;
}

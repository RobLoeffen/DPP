namespace DppApi.Domain.Entities.Pef;

public class PefFunctionalUnit
{
    public string Id { get; set; } = default!;
    public string Product { get; set; } = default!;
    public int Quantity { get; set; }
    public string Unit { get; set; } = default!;
    public PefMass Mass { get; set; } = new();
}

public class PefMass
{
    public double Value { get; set; }
    public string Unit { get; set; } = default!;
}

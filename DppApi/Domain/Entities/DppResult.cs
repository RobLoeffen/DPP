namespace DppApi.Domain.Entities;

public class DppResult
{
    public double TotalCo2 { get; set; }
    public string Unit => "kg CO2-eq";
    public string Method => "ISO 14067";
    public DppBreakdown Breakdown { get; set; } = new();
}

public class DppBreakdown
{
    public double Materials { get; set; }
    public double Production { get; set; }
    public double Transport { get; set; }
    public double Use { get; set; }
    public double EndOfLife { get; set; }
}

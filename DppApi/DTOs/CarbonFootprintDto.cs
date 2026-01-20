namespace DppApi.DTOs;

public class CarbonFootprintDto
{
    public double TotalCo2 { get; set; }
    public string Unit { get; set; } = "kg CO2-eq";
    public string Method { get; set; } = default!;
    public BreakdownDto Breakdown { get; set; } = new();
}

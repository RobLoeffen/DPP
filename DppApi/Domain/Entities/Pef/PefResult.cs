namespace DppApi.Domain.Entities.Pef;

public class PefResult
{
    public double TotalCo2Eq { get; set; }
    public string Unit => "kg CO2-eq";
    public string Method => "PEF met IPCC GWP100";
    public List<PefStageBreakdown> Breakdown { get; set; } = [];
}

public class PefStageBreakdown
{
    public string Stage { get; set; } = default!;
    public double Co2Eq { get; set; }
    public List<PefEmissionDetail> EmissionDetails { get; set; } = [];
}

public class PefEmissionDetail
{
    public string Substance { get; set; } = default!;
    public double OriginalValue { get; set; }
    public double GwpFactor { get; set; }
    public double Co2Eq { get; set; }
}

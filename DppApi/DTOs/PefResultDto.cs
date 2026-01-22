namespace DppApi.DTOs;

public class PefResultDto
{
    public double TotaalCo2Eq { get; set; }
    public string Eenheid { get; set; } = "kg CO2-eq";
    public string EenheidToelichting { get; set; } = "kilogram koolstofdioxide-equivalent berekend met IPCC GWP100 factoren";
    public string Methode { get; set; } = default!;
    public List<PefStageBreakdownDto> Uitsplitsing { get; set; } = [];
}

public class PefStageBreakdownDto
{
    public string Fase { get; set; } = default!;
    public double Co2Eq { get; set; }
    public List<PefEmissionDetailDto> EmissieDetails { get; set; } = [];
}

public class PefEmissionDetailDto
{
    public string Stof { get; set; } = default!;
    public double OrigineleWaarde { get; set; }
    public double GwpFactor { get; set; }
    public double Co2Eq { get; set; }
}

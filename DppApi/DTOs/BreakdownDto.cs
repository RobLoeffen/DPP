namespace DppApi.DTOs;

public class BreakdownDto
{
    public double Materials { get; set; }
    public double Production { get; set; }
    public double Transport { get; set; }
    public double Use { get; set; }
    public double EndOfLife { get; set; }
}

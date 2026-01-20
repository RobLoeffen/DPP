using DppApi.Domain.Entities;
using DppApi.Domain.Interfaces;

namespace DppApi.Services;

public class CarbonCalculator : ICarbonCalculator
{
    public DppResult Calculate(DppProduct product)
    {
        var materials = product.Materials.Sum(m => m.Kg * m.EmissionFactor);
        var production = product.Production.EnergyKwh * product.Production.ElectricityFactor;
        var transport = product.Transport.Sum(t => t.DistanceKm * t.WeightTon * t.EmissionFactor);
        var use = product.UsePhase is null ? 0 : product.UsePhase.Co2PerUse * product.UsePhase.Uses;
        var endOfLife = product.EndOfLife.KgWaste * product.EndOfLife.EmissionFactor;

        return new DppResult
        {
            TotalCo2 = Math.Round(materials + production + transport + use + endOfLife, 2),
            Breakdown = new DppBreakdown
            {
                Materials = Math.Round(materials, 2),
                Production = Math.Round(production, 2),
                Transport = Math.Round(transport, 2),
                Use = Math.Round(use, 2),
                EndOfLife = Math.Round(endOfLife, 2)
            }
        };
    }
}

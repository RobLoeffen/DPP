using DppApi.Domain.Entities.Pef;
using DppApi.Domain.Interfaces;

namespace DppApi.Services;

public class PefCalculator : IPefCalculator
{
    private static readonly Dictionary<string, double> GwpFactors = new()
    {
        ["CO2"] = 1.0,
        ["CO2_fossielen"] = 1.0,
        ["CO2_biogeen"] = 1.0,
        ["CH4"] = 28.0,      // Methaan
        ["N2O"] = 265.0      // Stikstofmonoxide 
    };

    public PefResult Calculate(PefProduct product)
    {
        var breakdown = new List<PefStageBreakdown>();
        double totalCo2Eq = 0;

        foreach (var stage in product.LifeCycleStages)
        {
            var stageBreakdown = new PefStageBreakdown
            {
                Stage = stage.Name,
                EmissionDetails = []
            };

            double stageCo2Eq = 0;

            foreach (var emission in stage.Emissions)
            {
                var gwpFactor = GetGwpFactor(emission.Substance);
                var co2Eq = emission.Value * gwpFactor;

                stageBreakdown.EmissionDetails.Add(new PefEmissionDetail
                {
                    Substance = emission.Substance,
                    OriginalValue = emission.Value,
                    GwpFactor = gwpFactor,
                    Co2Eq = Math.Round(co2Eq, 4)
                });

                stageCo2Eq += co2Eq;
            }

            stageBreakdown.Co2Eq = Math.Round(stageCo2Eq, 4);
            breakdown.Add(stageBreakdown);
            totalCo2Eq += stageCo2Eq;
        }

        return new PefResult
        {
            TotalCo2Eq = Math.Round(totalCo2Eq, 2),
            Breakdown = breakdown
        };
    }

    private static double GetGwpFactor(string substance)
    {
        return GwpFactors.TryGetValue(substance, out var factor) ? factor : 1.0;
    }
}

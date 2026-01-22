using DppApi.Domain.Entities.Pef;
using DppApi.DTOs;

namespace DppApi.Mapping;

public static class PefMapper
{
    public static PefResultDto ToDto(PefResult result) =>
        new()
        {
            TotaalCo2Eq = result.TotalCo2Eq,
            Methode = result.Method,
            Uitsplitsing = result.Breakdown.Select(b => new PefStageBreakdownDto
            {
                Fase = b.Stage,
                Co2Eq = b.Co2Eq,
                EmissieDetails = b.EmissionDetails.Select(e => new PefEmissionDetailDto
                {
                    Stof = e.Substance,
                    OrigineleWaarde = e.OriginalValue,
                    GwpFactor = e.GwpFactor,
                    Co2Eq = e.Co2Eq
                }).ToList()
            }).ToList()
        };
}

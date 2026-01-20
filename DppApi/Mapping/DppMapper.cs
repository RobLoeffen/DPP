using DppApi.Domain.Entities;
using DppApi.DTOs;

namespace DppApi.Mapping;

public static class DppMapper
{
    public static DppProductDto ToDto(DppProduct product) =>
        new()
        {
            Id = product.Id,
            Name = product.Name,
            Materials = product.Materials.Select(m => new MaterialDto
            {
                Name = m.Name,
                Kg = m.Kg
            })
        };

    public static CarbonFootprintDto ToDto(DppResult result) =>
        new()
        {
            TotalCo2 = result.TotalCo2,
            Method = result.Method,
            Breakdown = new BreakdownDto
            {
                Materials = result.Breakdown.Materials,
                Production = result.Breakdown.Production,
                Transport = result.Breakdown.Transport,
                Use = result.Breakdown.Use,
                EndOfLife = result.Breakdown.EndOfLife
            }
        };
}

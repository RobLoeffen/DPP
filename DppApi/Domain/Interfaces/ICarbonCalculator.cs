using DppApi.Domain.Entities;

namespace DppApi.Domain.Interfaces;

public interface ICarbonCalculator
{
    DppResult Calculate(DppProduct product);
}

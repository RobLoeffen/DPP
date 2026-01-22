using DppApi.Domain.Entities.Pef;

namespace DppApi.Domain.Interfaces;

public interface IPefCalculator
{
    PefResult Calculate(PefProduct product);
}

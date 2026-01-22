using DppApi.Domain.Entities.Pef;

namespace DppApi.Domain.Interfaces;

public interface IPefRepository
{
    PefProduct? GetById(string id);
}

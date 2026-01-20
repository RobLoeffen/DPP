using DppApi.Domain.Entities;

namespace DppApi.Domain.Interfaces;
public interface IDppRepository
{
    IEnumerable<DppProduct> GetAll();
    DppProduct? GetById(string id);
}

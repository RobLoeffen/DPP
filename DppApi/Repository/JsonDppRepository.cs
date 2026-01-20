using System.Text.Json;
using DppApi.Domain.Entities;
using DppApi.Domain.Interfaces;

namespace DppApi.Repository;

public class JsonDppRepository : IDppRepository
{
    private readonly List<DppProduct> _products;

    public JsonDppRepository()
    {
        var json = File.ReadAllText("Data/sample-products.json");
        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };
        _products = JsonSerializer.Deserialize<List<DppProduct>>(json, options)!;
    }

    public IEnumerable<DppProduct> GetAll() => _products.ToList();

    public DppProduct? GetById(string id) =>
        _products.FirstOrDefault(p => p.Id == id);
}

using System.Text.Json;
using DppApi.Domain.Entities.Pef;
using DppApi.Domain.Interfaces;

namespace DppApi.Repository;

public class JsonPefRepository : IPefRepository
{
    private readonly PefProduct _product;

    public JsonPefRepository()
    {
        var json = File.ReadAllText("Data/sample-products-pef.json");
        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };
        _product = JsonSerializer.Deserialize<PefProduct>(json, options)!;
    }

    public PefProduct? GetById(string id)
    {
        return _product.FunctionalUnit.Id == id ? _product : null;
    }
}

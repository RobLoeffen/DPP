using System.Security.Cryptography.Xml;

namespace DppApi.Domain.Entities;

public class DppProduct
{
    public required string Id { get; set; }
    public required string Name { get; set; }
    public List<Material> Materials { get; set; } = [];
    public Production Production { get; set; } = new();
    public List<Transport> Transport { get; set; } = [];
    public UsePhase? UsePhase { get; set; }
    public EndOfLife EndOfLife { get; set; } = new();
}

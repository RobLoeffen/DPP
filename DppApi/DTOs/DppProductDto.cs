namespace DppApi.DTOs;

public class DppProductDto
{
    public string Id { get; set; } = default!;
    public string Name { get; set; } = default!;

    public IEnumerable<MaterialDto> Materials { get; set; } = [];
}

public class MaterialDto
{
    public string Name { get; set; } = default!;
    public double Kg { get; set; }
}

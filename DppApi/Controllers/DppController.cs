using DppApi.Domain.Interfaces;
using DppApi.Mapping;
using Microsoft.AspNetCore.Mvc;

namespace DppApi.Controllers;

[ApiController]
[Route("api/dpp")]
public class DppController : ControllerBase
{
    private readonly IDppRepository _repository;
    private readonly ICarbonCalculator _calculator;

    public DppController(IDppRepository repository, ICarbonCalculator calculator)
    {
        _repository = repository;
        _calculator = calculator;
    }

    [HttpGet]
    public IActionResult GetAll()
    {
        var products = _repository.GetAll()
            .Select(DppMapper.ToDto);

        return Ok(products);
    }

    [HttpGet("{id}")]
    public IActionResult GetById(string id)
    {
        var product = _repository.GetById(id);
        if (product is null) return NotFound();

        return Ok(DppMapper.ToDto(product));
    }

    [HttpGet("{id}/carbon-footprint")]
    public IActionResult GetCarbonFootprint(string id)
    {
        var product = _repository.GetById(id);
        if (product is null) return NotFound();

        var result = _calculator.Calculate(product);
        return Ok(DppMapper.ToDto(result));
    }
}

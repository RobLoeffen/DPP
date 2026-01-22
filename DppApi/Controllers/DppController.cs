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
    private readonly IPefRepository _pefRepository;
    private readonly IPefCalculator _pefCalculator;

    public DppController(
        IDppRepository repository, 
        ICarbonCalculator calculator,
        IPefRepository pefRepository,
        IPefCalculator pefCalculator)
    {
        _repository = repository;
        _calculator = calculator;
        _pefRepository = pefRepository;
        _pefCalculator = pefCalculator;
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
    public IActionResult GetCarbonFootprint(string id, [FromQuery] string method = "iso14067")
    {
        if (method.ToLower() == "pef")
        {
            var pefProduct = _pefRepository.GetById(id);
            if (pefProduct is null) return NotFound();

            var pefResult = _pefCalculator.Calculate(pefProduct);
            return Ok(PefMapper.ToDto(pefResult));
        }
        else
        {
            var product = _repository.GetById(id);
            if (product is null) return NotFound();

            var result = _calculator.Calculate(product);
            return Ok(DppMapper.ToDto(result));
        }
    }

    [HttpGet("calculation-methods")]
    public IActionResult GetAvailableMethods()
    {
        return Ok(new[] 
        { 
            new { Methode = "ISO14067", Omschrijving = "Vereenvoudigde carbon footprint methode" },
            new { Methode = "PEF", Omschrijving = "Product Environmental Footprint met IPCC GWP100 factoren" }
        });
    }
}

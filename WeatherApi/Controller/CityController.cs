using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using WeatherApi.Data.DTOs;
using WeatherApi.Models;
using WeatherApi.Service.Interfaces;

namespace WeatherApi.Controller;

[ApiController]
[Route("api")]

public class CityController : ControllerBase
{
    private ICityService _cityService;
    private IMapper _mapper;

    public CityController(ICityService cityService, IMapper mapper)
    {
        _cityService = cityService;
        _mapper = mapper;
    }

    /// <summary>
    /// Cria uma cidade
    //// </summary>
    [HttpPost("register-city")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public IActionResult PostWeather(
        [FromBody] PostCityDTO postCityDTO)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var cityConverter = _mapper.Map<City>(postCityDTO);
        var citySave = _cityService.Save(cityConverter);

        return CreatedAtAction(nameof(GetCityForId), new { id = citySave.IdCity }, citySave);
    }

    /// <summary>
    /// Lista todas as cidades
    //// </summary>
    [HttpGet("cities/all")]
    public IActionResult GetAll()
    {
        var cityData = _cityService.FindAll();

        return Ok(cityData);

    }

    /// <summary>
    /// Lista uma cidade por id
    /// </summary>
    [HttpGet("{id}")]
    public IActionResult GetCityForId(Guid id)
    {
        var city = _cityService.FindById(id);
        return city is null ? NotFound("Weather not found") : Ok(city);
    }

    //[HttpDelete("{id}")]
    //public IActionResult DeleteCity(long id)
    //{

    //    _cityService.DeleteById(id);
    //    return NoContent();
    //}
}

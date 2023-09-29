using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using WeatherApi.Data.DTOs;
using WeatherApi.Models;
using WeatherApi.Service;
using WeatherApi.Service.Interfaces;

namespace WeatherApi.Controller;

[ApiController]
[Route("[controller]/api")]
public class WeatherController : ControllerBase
{
    private IWeatherService _weatherService;
    private IMapper _mapper;

    public WeatherController(IWeatherService weatherService, IMapper mapper)
    {
        _weatherService = weatherService;
        _mapper = mapper;
    }

    /// <summary>
    /// Cria um dado metereológico
    /// </summary>
    [HttpPost("register-weather")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public IActionResult PostWeather(
        [FromBody] WeatherRequestDTO postWeatherDTO)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var weatherConverter = _mapper.Map<Weather>(postWeatherDTO);
        var weatherSave = _weatherService.Save(weatherConverter);

        return CreatedAtAction(nameof(GetWeatherById), new { id = weatherSave.IdWeather }, weatherSave);
    }

    /// <summary>
    /// Lista todos registros de dados meteorológicos
    /// </summary>
    [HttpGet("weather-all")]
    public IEnumerable<Weather> GetWeatherWithWeatherData()
    {
        return _weatherService.FindAll();
    }

    /// <summary>
    /// Lista 10 registros de dados meteorológicos por página quando NÃO pesquisar a cidade
    /// </summary>
    [HttpGet("list-all-page")]
    public IEnumerable<Weather> GetWithWeatherData(int pageNumber = 1, int pageSize = 10)
    {
        return _weatherService.FindAllPage(pageNumber, pageSize);
    }

    /// <summary>
    /// Lista 10 registros de dados meteorológicos por página de todas as cidades quando pesquisar a cidade
    /// </summary>
    [HttpGet("{cityName}/list-all-page")]
    public IEnumerable<Weather> GetAllByName([FromQuery] string cityName, int page, int pageSize)
    {
        return _weatherService.FindAllPageByNameCity(cityName, page, pageSize);
    }

    /// <summary>
    /// Lista dado meteorológico do dia atual e de mais 6 dias consecutivos.
    /// </summary>
    [HttpGet("weather-next-7-days")]
    public IActionResult GetWeatherForNext7Days()
    {
        var weatherData = _weatherService.GetWeatherForNext7Days();
        return Ok(weatherData);
    }

    /// <summary>
    /// Lista registros de um estado pelo id
    /// </summary>
    [HttpGet("{id}")]
    public IActionResult GetWeatherById(Guid id)
    {
        var weather = _weatherService.FindById(id);
        return weather is null ? NotFound("Weather not found") : Ok(weather);
    }

    //[HttpPut("{id}")]
    //public IActionResult PutWeather(Guid id, [FromBody] PutWeatherDTO weatherDto)
    //{

    //    Weather weather = new Weather();
    //    //var weatherConverter = _mapper.Map<Weather>(weatherDto);

    //    var weatherConverter = _mapper.Map(weatherDto, weather);
    //    var weatherEdit = _weatherService.Update(id, weatherConverter);

    //    return Ok(weatherEdit);
    //    //var weather = _weatherContext.Weathers.FirstOrDefault(
    //    //    weather => weather.IdWeather == id);
    //    //if (weather == null) return NotFound();
    //    //_mapper.Map(weatherDto, weather);
    //    //_weatherContext.SaveChanges();

    //    //return NoContent();
    //}

    //[HttpDelete("{id}")]
    //public IActionResult DeleteWeather(Guid id)
    //{

    //    _weatherService.DeleteById(id);
    //    return NoContent();
    //}
}

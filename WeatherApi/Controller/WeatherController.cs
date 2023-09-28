using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using WeatherApi.Data.DTOs;
using WeatherApi.Models;
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
    /// Cria um clima
    //// </summary>
    [HttpPost("register-weather")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public IActionResult PostWeather(
        [FromBody] PostWeatherDTO postWeatherDTO)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var weatherConverter = _mapper.Map<Weather>(postWeatherDTO);
        var weatherSave = _weatherService.Save(weatherConverter);

        return CreatedAtAction(nameof(GetWeatherForId), new { id = weatherSave.IdWeather }, weatherSave);
    }


    /// <summary>
    /// Lista 10 registros por página quando NÃO pesquisar a cidade
    /// </summary>
    //[HttpGet]
    //public IActionResult GetAll([FromQuery] int page, int pageSize)
    //{
    //    var weatherData = _weatherService.FindAllPage(page, pageSize);

    //    return Ok(weatherData);

    //}

    ///// <summary>
    ///// Lista 10 registros por página de todas as cidades quando pesquisar a cidade
    ///// </summary>
    //[HttpGet("{cityName}")]
    //public IActionResult GetAll([FromQuery] string cityName, int page, int pageSize)
    //{
    //    var weatherData = _weatherService.FindAllPageByNameCity(cityName, page, pageSize);

    //    return Ok(weatherData);

    //}

    /// <summary>
    /// Lista registros de um estado pelo id
    /// </summary>
    [HttpGet("{id}")]
    public IActionResult GetWeatherForId(Guid id)
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

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
    /// Lista 10 registros de dados meteorológicos por página, quando NÃO pesquisar uma cidade, em ordem descrescente, por data.
    /// </summary>
    [HttpGet("list-all-page")]
    public IEnumerable<Weather> GetWithWeatherData(int pageNumber = 1, int pageSize = 10)
    {
        return _weatherService.FindAllPage(pageNumber, pageSize);
    }

    /// <summary>
    /// Lista 10 registros de dados meteorológicos por página, quando PESQUISAR uma cidade, em ordem descrescente por data.
    /// </summary>
    [HttpGet("{cityName}/list-all-page")]
    public IEnumerable<Weather> GetAllByName([FromQuery] string cityName, int page, int pageSize)
    {
        return _weatherService.FindAllPageByNameCity(cityName, page, pageSize);
    }

    /// <summary>
    /// Lista o dado meteorológico do dia atual e de mais 6 dias consecutivos de uma cidade, em ordem crescente por data.
    /// </summary>
    [HttpGet("{cityName}/weather-next-7-days")]
    public IActionResult GetWeatherForNext7Days([FromRoute] string cityName)
    {
        var weatherData = _weatherService.GetWeatherForNext7Days(cityName);
        return Ok(weatherData);
    }

    /// <summary>
    /// Lista registros de dados meteorológicos pelo id.
    /// </summary>
    [HttpGet("{id}")]
    public IActionResult GetWeatherById(Guid id)
    {
        var weather = _weatherService.FindById(id);
        return weather is null ? NotFound("Weather not found") : Ok(weather);
    }

    /// <summary>
    /// Atualiza um registro de dado meteorológico.
    /// </summary>
    [HttpPut("{id}")]
    public IActionResult PutWeather(Guid id, [FromBody] WeatherRequestDTO weatherDto)
    {
        var weather = _mapper.Map<Weather>(weatherDto);
        var updatedWeather = _weatherService.Update(id, weather);

        return weatherDto == null ? BadRequest("Invalid weather data.") : (updatedWeather == null ? NotFound("Weather data not found.") : Ok(updatedWeather));
    }

    /// <summary>
    /// Exclui um registro de dado meteorológico.
    /// </summary>
    [HttpDelete("{id}")]
    public IActionResult DeleteWeather(Guid id)
    {
        _weatherService.DeleteById(id);
        return NoContent();
    }

}

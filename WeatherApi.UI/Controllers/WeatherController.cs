using Microsoft.AspNetCore.Mvc;
using System.Data;
using WeatherApi.Data.DTOs;
using WeatherApi.Entity;
using WeatherApi.Service.Interfaces;
using WeatherApi.UI.Middlewares.Exceptions;

namespace WeatherApi.UI.Middlewares;

[ApiController]
[Route("[controller]/api")]
public class WeatherController : ControllerBase
{
    private IWeatherService _weatherService;

    public WeatherController(IWeatherService weatherService)
    {
        _weatherService = weatherService;
    }

    /// <summary>
    /// Cria um dado metereol�gico
    /// </summary>
    [HttpPost("register-weather")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<IActionResult> PostWeather(
        [FromBody] WeatherRequestDTO postWeatherDTO)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var weatherSave = await _weatherService.Save(postWeatherDTO);

        return CreatedAtAction(nameof(GetWeatherById), new { id = weatherSave.IdWeather }, weatherSave);
    }

    /// <summary>
    /// Lista todos registros de dados meteorol�gicos
    /// </summary>
    [HttpGet("weather-all")]
    public async Task<IEnumerable<Weather>> GetWeatherWithWeatherData()
    {
        var weatherAll = await _weatherService.FindAll();

        if (!ModelState.IsValid)
        {
            throw new DBConcurrencyException("Erro ao acessar a base de dados");
        }

        return weatherAll.ToList();

    }

    /// <summary>
    /// Lista 10 registros de dados meteorol�gicos por p�gina, quando N�O pesquisar uma cidade, em ordem descrescente, por data.
    /// </summary>
    [HttpGet("list-all-page")]
    public async Task<IEnumerable<Weather>> GetWithWeatherData(int pageNumber = 1, int pageSize = 10)
    {
        return await _weatherService.FindAllPage(pageNumber, pageSize);
    }

    /// <summary>
    /// Lista 10 registros de dados meteorol�gicos por p�gina, quando PESQUISAR uma cidade, em ordem descrescente por data.
    /// </summary>
    [HttpGet("{cityName}/list-all-page")]
    public async Task<IEnumerable<Weather>> GetAllByName([FromRoute] string cityName, int pageNumber = 1, int pageSize = 10)
    {
        return await _weatherService.FindAllPageByNameCity(cityName, pageNumber, pageSize);
    }

    /// <summary>
    /// Lista o dado meteorol�gico do dia atual e de mais 6 dias consecutivos de uma cidade, em ordem crescente por data.
    /// </summary>
    [HttpGet("{cityName}/weather-next-7-days")]
    public async Task<IActionResult> GetWeatherForNext7Days([FromRoute] string cityName)
    {
        var weatherData = await _weatherService.GetWeatherForNext7Days(cityName);
        return Ok(weatherData);
    }

    /// <summary>
    /// Lista registros de dados meteorol�gicos pelo id.
    /// </summary>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetWeatherById(Guid id)
    {
        var weather = await _weatherService.FindById(id);

        if (weather is null)
        {
            throw new NotFoundException("Weather n�o encontrado");
        }

        return Ok(weather);
    }

    /// <summary>
    /// Atualiza um registro de dado meteorol�gico.
    /// </summary>
    [HttpPut("{id}")]
    public async Task<IActionResult> PutWeather(Guid id, [FromBody] WeatherRequestDTO weatherDto)
    {
        var updatedWeather = await _weatherService.Update(id, weatherDto);

        return weatherDto == null ? BadRequest("Invalid weather data.") : (updatedWeather == null ? NotFound("Weather data not found.") : Ok(updatedWeather));
    }

    /// <summary>
    /// Exclui um registro de dado meteorol�gico.
    /// </summary>
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteWeather(Guid id)
    {
        await _weatherService.DeleteById(id);
        return NoContent();
    }

}

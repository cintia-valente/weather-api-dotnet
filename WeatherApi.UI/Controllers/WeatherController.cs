using Microsoft.AspNetCore.Mvc;
using WeatherApi.Data.DTOs;
using WeatherApi.Entity;
using WeatherApi.Service.Interfaces;

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
    /// Cria um dado metereológico
    /// </summary>
    [HttpPost("register-weather")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<IActionResult> PostWeather(
        [FromBody] WeatherRequestDTO postWeatherDTO)
    {
        var weatherSave = await _weatherService.Save(postWeatherDTO);

        return CreatedAtAction(nameof(GetWeatherById), new { id = weatherSave.IdWeather }, weatherSave);
    }

    /// <summary>
    /// Lista todos registros de dados meteorológicos
    /// </summary>
    [HttpGet("weather-all")]
    public async Task<IEnumerable<Weather>> GetWeatherWithWeatherData()
    {
        var weatherAll = await _weatherService.FindAll();

        return weatherAll.ToList();

    }

    /// <summary>
    /// Lista 10 registros de dados meteorológicos por página, quando NÃO pesquisar uma cidade, em ordem descrescente, por data.
    /// </summary>
    [HttpGet("list-all-page")]
    public async Task<IEnumerable<Weather>> GetWithWeatherData(int pageNumber = 1, int pageSize = 10)
    {
        return await _weatherService.FindAllPage(pageNumber, pageSize);
    }

    /// <summary>
    /// Lista 10 registros de dados meteorológicos por página, quando PESQUISAR uma cidade, em ordem descrescente por data.
    /// </summary>
    [HttpGet("{cityName}/list-all-page")]
    public async Task<IEnumerable<Weather>> GetAllByName([FromRoute] string cityName, int pageNumber = 1, int pageSize = 10)
    {
        return await _weatherService.FindAllPageByNameCity(cityName, pageNumber, pageSize);
    }

    /// <summary>
    /// Lista o dado meteorológico do dia atual e de mais 6 dias consecutivos de uma cidade, em ordem crescente por data.
    /// </summary>
    [HttpGet("{cityName}/weather-next-7-days")]
    public async Task<IActionResult> GetWeatherForNext7Days([FromRoute] string cityName)
    {
        var weatherData = await _weatherService.GetWeatherForNext7Days(cityName);

        return Ok(weatherData);
    }

    /// <summary>
    /// Lista registros de dados meteorológicos pelo id.
    /// </summary>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetWeatherById(Guid id)
    {
        var weather = await _weatherService.FindById(id);

        return Ok(weather);
    }

    /// <summary>
    /// Atualiza um registro de dado meteorológico.
    /// </summary>
    [HttpPut("{id}")]
    public async Task<IActionResult> PutWeather(Guid id, [FromBody] WeatherRequestDTO weatherDto)
    {
        await _weatherService.Update(id, weatherDto);

        return Ok();
        //return weatherDto == null ? BadRequest("Invalid weather data.") : (updatedWeather == null ? NotFound("Weather data not found.") : Ok(updatedWeather));
    }

    /// <summary>
    /// Exclui um registro de dado meteorológico.
    /// </summary>
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteWeather(Guid id)
    {
        await _weatherService.DeleteById(id);

        return NoContent();
    }

}

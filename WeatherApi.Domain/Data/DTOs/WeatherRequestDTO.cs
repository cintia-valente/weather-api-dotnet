using System.ComponentModel.DataAnnotations;
using WeatherApi.Models.Enums;
using WeatherApi.Models;

namespace WeatherApi.Data.DTOs;

public class WeatherRequestDTO
{
    public Guid IdWeather { get; set; }
    public DateTime Date { get; set; }
    public int MaxTemperature { get; set; }
    public int MinTemperature { get; set; }
    public double Precipitation { get; set; }
    public double Humidity { get; set; }
    public double WindSpeed { get; set; }
    public DayTimeEnum DayTime { get; set; }
    public NightTimeEnum NightTime { get; set; }

    public Guid IdCity { get; set; }
    public CityResponseDto? City { get; set; }
}

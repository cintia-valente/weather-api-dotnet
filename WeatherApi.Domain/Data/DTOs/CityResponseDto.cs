using System.ComponentModel.DataAnnotations;
using WeatherApi.Models.Enums;
using WeatherApi.Models;

namespace WeatherApi.Data.DTOs;

public class CityResponseDto
{
    public Guid IdCity { get; set; }
    public string Name { get; set; }
}

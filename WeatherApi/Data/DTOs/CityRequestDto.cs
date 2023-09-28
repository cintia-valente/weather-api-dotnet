using System.ComponentModel.DataAnnotations;

namespace WeatherApi.Data.DTOs;

public class CityRequestDto
{
    [Key]
    public long IdCity { get; set; }
    [Required]

    public string Name { get; set; }
}

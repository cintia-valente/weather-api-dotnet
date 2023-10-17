using System.ComponentModel.DataAnnotations;

namespace WeatherApi.Data.DTOs;

public class CityRequestDto
{
    [Key]
    public Guid IdCity { get; set; }
    [Required]

    public string Name { get; set; }
}

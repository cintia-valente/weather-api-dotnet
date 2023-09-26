using System.ComponentModel.DataAnnotations;

namespace WeatherApi.Models;

public class City
{
    [Key]
    public long IdCity { get; set; }

    [Required(ErrorMessage = "O nome da cidade é obrigatório")]
    public string Name { get; set; }

    public List<Weather> WeatherDataList { get; set; }

}

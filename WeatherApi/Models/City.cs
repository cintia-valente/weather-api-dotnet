using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WeatherApi.Models;

public class City
{
    [Key]
    public Guid IdCity { get; set; }

    [Required(ErrorMessage = "O nome da cidade é obrigatório")]
    public string Name { get; set; }

    [ForeignKey("IdCity")]
    public virtual List<Weather> WeatherDataList { get; set; } = new List<Weather>();
   

}

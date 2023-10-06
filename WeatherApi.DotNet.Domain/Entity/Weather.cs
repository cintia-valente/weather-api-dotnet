using System.ComponentModel.DataAnnotations;
using WeatherApi.Entity.Enums;

namespace WeatherApi.Entity;

public class Weather
{
    [Key]
    public Guid IdWeather { get; set; }
   
    public DateTime Date { get; set; }
    public int MaxTemperature { get; set; }
    public int MinTemperature { get; set; }
    public double Precipitation { get; set; }
    public double Humidity { get; set; }
    public double WindSpeed { get; set; }
    
    [EnumDataType(typeof(DayTimeEnum))]
    public DayTimeEnum DayTime { get; set; }
    
    [EnumDataType(typeof(DayTimeEnum))]
    public NightTimeEnum NightTime { get; set; }

    public Guid IdCity { get; set; }
    public City? City { get; set; }
}

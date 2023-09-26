using System.ComponentModel.DataAnnotations;
using WeatherApi.Models.Enums;

namespace WeatherApi.Models;

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
    public DayTimeEnum DayTime { get; set; }
    public NightTimeEnum NightTime { get; set; }

    public City City { get; set; }
}

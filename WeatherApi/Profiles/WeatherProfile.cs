using AutoMapper;
using WeatherApi.Data.DTOs;
using WeatherApi.Models;

namespace WeatherApi.Profiles;

public class WeatherProfile : Profile
{
    public WeatherProfile()
    {
        CreateMap<PostWeatherDTO, Weather>();
        CreateMap<PutWeatherDTO, Weather>();
       // CreateMap<PutWeatherDTO, Weather>();
    }
}

using AutoMapper;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using WeatherApi.Data.DTOs;
using WeatherApi.Entity;

namespace WeatherApi.Profiles;

public class WeatherProfile : Profile
{
    public WeatherProfile()
    {
        CreateMap<WeatherRequestDTO, Weather>()
            .ForMember(dest => dest.IdWeather, opt => opt.MapFrom(src => Guid.NewGuid()));
        CreateMap<Weather, WeatherRequestDTO>()
           .ForMember(dest => dest.IdWeather, opt => opt.MapFrom(src => Guid.NewGuid()));
        CreateMap<EntityEntry<Weather>, Weather>();
    }
}

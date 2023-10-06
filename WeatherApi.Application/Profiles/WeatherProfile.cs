using AutoMapper;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace WeatherApi.Profiles;

public class WeatherProfile : Profile
{
    public WeatherProfile()
    {
        CreateMap<WeatherRequestDTO, Weather>()
            .ForMember(dest => dest.IdWeather, opt => opt.MapFrom(src => Guid.NewGuid()));
        CreateMap<Weather, WeatherRequestDTO>()
           .ForMember(dest => dest.IdWeather, opt => opt.MapFrom(src => Guid.NewGuid()));
        CreateMap<EntityEntry<Weather?>, Weather>();

        // CreateMap<PutWeatherDTO, Weather>();
    }
}

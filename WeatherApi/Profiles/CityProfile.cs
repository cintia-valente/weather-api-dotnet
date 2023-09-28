using AutoMapper;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using WeatherApi.Data.DTOs;
using WeatherApi.Models;

namespace WeatherApi.Profiles;

public class CityProfile : Profile
{
    public CityProfile()
    {


        CreateMap<CityRequestDto, City>()
             .ForMember(dest => dest.IdCity, opt => opt.MapFrom(src => Guid.NewGuid()));
        CreateMap<CityResponseDto, City>();

        CreateMap<EntityEntry<City?>, City>();
    
    }
}

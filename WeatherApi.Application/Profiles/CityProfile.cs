using AutoMapper;
using Microsoft.EntityFrameworkCore.ChangeTracking;


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

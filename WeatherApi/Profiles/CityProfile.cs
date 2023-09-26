using AutoMapper;
using WeatherApi.Data.DTOs;
using WeatherApi.Models;

namespace WeatherApi.Profiles;

public class CityProfile : Profile
{
    public CityProfile()
    {
        CreateMap<PostCityDTO, City>();
    }
}

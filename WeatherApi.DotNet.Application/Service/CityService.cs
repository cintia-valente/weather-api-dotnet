using WeatherApi.Entity;
using WeatherApi.Service.Interfaces;
using WeatherApi.Repository.Interfaces;
using WeatherApi.Data.DTOs;
using WeatherApi.DotNet.Application.Exceptions;
using WeatherApi.Entity.Enums;
using WeatherApi.Repository;
using AutoMapper;

namespace WeatherApi.Service;

public class CityService : ICityService
{
    private readonly ICityRepository _cityRepository;
    private readonly IMapper _mapper;

    public CityService(ICityRepository cityRepository, IMapper mapper)
    {
        _cityRepository = cityRepository;
        _mapper = mapper;
    }

    public async Task<City> Save(CityRequestDto cityDto)
    {
        var cityConverter = _mapper.Map<City>(cityDto);

        var citySaved = await _cityRepository.Save(cityConverter);

        return citySaved;
    }

    public async Task<IEnumerable<City>> FindAll()
    {
        return await _cityRepository.FindAll();
    }

    public async Task<City> FindById(Guid id)
    {
        var cityById = await _cityRepository.FindById(id);
        return cityById;
    }

}

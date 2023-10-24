using AutoMapper;
using WeatherApi.Data.DTOs;
using WeatherApi.Entity;
using WeatherApi.Entity.Enums;
using WeatherApi.Repository.Interfaces;
using WeatherApi.Service.Interfaces;

namespace WeatherApi.Service;

public class WeatherService : IWeatherService
{
    private IWeatherRepository _weatherRepository;
    private ICityRepository _cityRepository;
    private IMapper _mapper;

    public WeatherService(IWeatherRepository weatherRepository, ICityRepository cityRepository, IMapper mapper)
    {
        _weatherRepository = weatherRepository;
        _cityRepository = cityRepository;
        _mapper = mapper;
    }

    public async Task<Weather> Save(WeatherRequestDTO weatherDto) 
    {
        var weatherConverter = _mapper.Map<Weather>(weatherDto);
        weatherConverter.City = await _cityRepository.FindById(weatherDto.IdCity);

        if (!Enum.IsDefined(typeof(DayTimeEnum), weatherDto.DayTime) ||
            !Enum.IsDefined(typeof(NightTimeEnum), weatherDto.NightTime))
        {
            throw new ArgumentException("Valores inválidos para enums DayTime e/ou NightTime.");
        }
       
        var weatherSaved = await _weatherRepository.Save(weatherConverter);
       
        return weatherSaved;
    }
    
    public async Task<Weather> FindById(Guid id)
    {
        return await _weatherRepository.FindById(id);
    }

    public async Task<IEnumerable<Weather>> FindAll()
    {
        return await _weatherRepository.FindAll();
    }

    public async Task<IEnumerable<Weather>> FindAllPage(int page, int pageSize)
    {
        return await _weatherRepository.FindAllByOrderByDateDesc(page, pageSize);
    }

    public async Task<IEnumerable<Weather>> FindAllPageByNameCity(string cityName, int page, int pageSize)
    {
        return await _weatherRepository.FindAllByCityName(cityName, page, pageSize);
    }

    public async Task<IEnumerable<Weather>> GetWeatherForNext7Days(string cityName)
    {
        return await _weatherRepository.FindByCityNextSixWeek(cityName);
    }

    public async Task<Weather> Update(Guid idWeatherData, WeatherRequestDTO weatherDto)
    {
        var data = await FindById(idWeatherData);

        weatherDto.IdWeather = idWeatherData;

        if (!Enum.IsDefined(typeof(DayTimeEnum), weatherDto.DayTime) ||
        !Enum.IsDefined(typeof(NightTimeEnum), weatherDto.NightTime))
        {
            throw new ArgumentException("Valores inválidos para enums DayTime e/ou NightTime.");
        }

        var weather = _mapper.Map<Weather>(weatherDto);

        await _weatherRepository.Update(idWeatherData, weather);

        return data;
    }

    public async Task<bool> DeleteById(Guid idWheaterData)
    {
        return await _weatherRepository.DeleteById(idWheaterData);
    }

}

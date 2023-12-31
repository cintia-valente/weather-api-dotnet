﻿using AutoMapper;
using WeatherApi.Data.DTOs;
using WeatherApi.DotNet.Application.Exceptions;
using WeatherApi.Entity;
using WeatherApi.Entity.Enums;
using WeatherApi.Repository.Interfaces;
using WeatherApi.Service.Interfaces;

namespace WeatherApi.Service;

public class WeatherService : IWeatherService
{
    private readonly IWeatherRepository _weatherRepository;
    private readonly ICityRepository _cityRepository;
    private readonly IMapper _mapper;

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

        if (weatherConverter.City is null)
        {
            throw new NotFoundException("Erro ao salvar");
        }

        if (!Enum.IsDefined(typeof(DayTimeEnum), weatherDto.DayTime) ||
            !Enum.IsDefined(typeof(NightTimeEnum), weatherDto.NightTime))
        {
            throw new ArgumentCostumerException("Valores inválidos para enums DayTime e/ou NightTime.");
        }

        var weatherSaved = await _weatherRepository.Save(weatherConverter);
       
        return weatherSaved;
    }
    public async Task<Weather> FindById(Guid id, bool tracking = true)
    {
        var weatherData = await _weatherRepository.FindById(id, tracking);

        if (weatherData is null)
        {
            throw new NotFoundException("Weather não encontrado");
        }

        return weatherData;
    }

    public async Task<IEnumerable<Weather>> FindAll()
    {
        var weatherData = await _weatherRepository.FindAll(); 

        return weatherData;
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

    public async Task Update(Guid idWeatherData, WeatherRequestDTO weatherDto)
    {
        var data = await FindById(idWeatherData, false);

        Console.WriteLine(data);

        if (data is null)
        {
            throw new NotFoundException("Weather não encontrado");
        }

        weatherDto.IdWeather = idWeatherData;

        var weather = _mapper.Map<Weather>(weatherDto);

        await _weatherRepository.Update(idWeatherData, weather);
    }

    public async Task<bool> DeleteById(Guid idWheaterData)
    {
        return await _weatherRepository.DeleteById(idWheaterData);
    }

}

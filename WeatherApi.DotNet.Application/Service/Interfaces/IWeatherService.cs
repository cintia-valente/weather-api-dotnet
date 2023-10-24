﻿using WeatherApi.Data.DTOs;
using WeatherApi.Entity;

namespace WeatherApi.Service.Interfaces;

public interface IWeatherService
{ 
 //   Task<Weather> Save(Weather weather);
    Task<Weather> Save(WeatherRequestDTO weatherDto);
    Task<IEnumerable<Weather>> FindAll();
    Task<IEnumerable<Weather>> FindAllPageByNameCity(string cityName, int page, int pageSize);

    Task<Weather> FindById(Guid id);
    //Weather FindById(Guid id);

    Task<IEnumerable<Weather>> FindAllPage(int page, int pageSize);
    Task<IEnumerable<Weather>> GetWeatherForNext7Days(string cityName);

    //Task<Weather> Update(Guid idWheaterData, Weather weather);
    Task<Weather> Update(Guid idWeatherData, WeatherRequestDTO weatherDto);
    //Weather Update(Guid idWheaterData, Weather weather);

    Task<bool> DeleteById(Guid idWheaterData);
}

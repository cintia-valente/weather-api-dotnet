﻿using WeatherApi.Entity;

namespace WeatherApi.Service.Interfaces;

public interface IWeatherService
{ 
    Task<Weather> Save(Weather weather);
    Task<IEnumerable<Weather>> FindAll();
    IEnumerable<Weather> FindAllPageByNameCity(string cityName, int page, int pageSize);
    Weather FindById(Guid id);

    IEnumerable<Weather> FindAllPage(int page, int pageSize);
    IEnumerable<Weather> GetWeatherForNext7Days(string cityName);

    Weather Update(Guid idWheaterData, Weather weather);

    bool DeleteById(Guid idWheaterData);


}

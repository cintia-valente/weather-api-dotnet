﻿using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Runtime.InteropServices;
using WeatherApi.Models;

namespace WeatherApi.Repository.Interfaces
{
    public interface IWeatherRepository
    {
        Weather Save(Weather weather);

        IQueryable<Weather> FindAll();
        IEnumerable<Weather> FindAllByOrderByDateDesc(int page, int pageSize);
        IQueryable<Weather> FindAllByCityName(string cityName, int page, int pageSize);
        IQueryable<Weather> FindByCityNextSixWeek(string cityName);
       
        Weather? FindByID(Guid idWeather);

        IEnumerable<Weather> FindByDates(List<DateTime> dates);

        void Update();
        bool DeleteById(Guid idWheater);
    }
}

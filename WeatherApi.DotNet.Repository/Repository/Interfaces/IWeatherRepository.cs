using WeatherApi.Entity;

namespace WeatherApi.Repository.Interfaces;

public interface IWeatherRepository
{
    Task<Weather> Save(Weather weather);

    Task<IQueryable<Weather>> FindAll();

    Task<IEnumerable<Weather>> FindAllByOrderByDateDesc(int page, int pageSize);

    Task<IQueryable<Weather>> FindAllByCityName(string cityName, int page, int pageSize);
    Task<IQueryable<Weather>> FindByCityNextSixWeek(string cityName);
   // Task<Weather?> FindById(Guid idWeather);

    Weather? FindById(Guid idWeather);
   // IEnumerable<Weather> FindByDates(List<DateTime> dates);

    //Task Update(Guid idWheaterData, Weather weather);
    void Update(Guid idWheaterData, Weather weather);

    Task<bool> DeleteById(Guid idWeather);
}

using WeatherApi.Entity;

namespace WeatherApi.Repository.Interfaces;

public interface IWeatherRepository
{
    Task<Weather> Save(Weather weather);

    Task<IQueryable<Weather>> FindAll();
    IEnumerable<Weather> FindAllByOrderByDateDesc(int page, int pageSize);
    IQueryable<Weather> FindAllByCityName(string cityName, int page, int pageSize);
    IQueryable<Weather> FindByCityNextSixWeek(string cityName);
   
    Weather? FindById(Guid idWeather);

    IEnumerable<Weather> FindByDates(List<DateTime> dates);

    void Update(Guid idWheaterData, Weather weather);
    bool DeleteById(Guid idWheater);
}

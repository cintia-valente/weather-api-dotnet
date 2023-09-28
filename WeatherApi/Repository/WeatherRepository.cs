using AutoMapper;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using WeatherApi.Data;
using WeatherApi.Models;
using WeatherApi.Repository.Interfaces;

namespace WeatherApi.Repository
{
    public class WeatherRepository : IWeatherRepository
    {
        private readonly WeatherContext _context; // Substitua pelo seu contexto de banco de dados
        private IMapper _mapper;
        public WeatherRepository(WeatherContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public Weather Save(Weather weather)
        {
            EntityEntry<Weather?> weatherEntity = _context.WeatherData.Add(weather);
           
            _context.SaveChanges();
            var weatherConvert = _mapper.Map<Weather>(weatherEntity.Entity);
            return weatherConvert;
        }

        public IEnumerable<Weather> FindAll()
        {
            return _context.WeatherData.ToList();
        }

        public IEnumerable<Weather> FindAllByOrderByDateDesc(int page, int pageSize)
        {
            var allWeather = _context.WeatherData
           .OrderByDescending(data => data.Date); // Ordenar por data em ordem descendente

            int skipAmount = (page - 1) * pageSize;

            return allWeather.Skip(skipAmount).Take(pageSize);
           
        }

        public IEnumerable<Weather> FindAllByCityNameIgnoreCase(string cityName, int page, int pageSize)
        {
           // var filteredWeather = _context.WeatherData
           //.Where(w => w.City == cityName)
           //.OrderByDescending(w => w.Date);

            var filteredWeather = _context.WeatherData
                .Where(data => data.City.Name.Equals(cityName, StringComparison.OrdinalIgnoreCase));

            int skipAmount = (page - 1) * pageSize;

            return filteredWeather.Skip(skipAmount).Take(pageSize);
        }

        public IEnumerable<Weather> FindByCityNextSixWeek(string cityName)
        {
            DateTime currentDate = DateTime.Today;
            DateTime endDate = currentDate.AddDays(6);  // Dia atual mais 6 dias

            var filteredWeather = _context.WeatherData
                .Where(data => data.City.Name == cityName && data.Date >= currentDate && data.Date <= endDate)
                .OrderBy(data => data.Date);  // Ordenar por data em ordem ascendente

            return filteredWeather;
        }

        public Weather? FindByID(Guid idWeather)
        {
            return _context.WeatherData.FirstOrDefault(metData => metData.IdWeather == idWeather);
           
        }

        public void Update()
        {
            _context.SaveChanges();
        }

        public bool DeleteById(Guid idWeather)
        {
            var weatherToDelete = _context.WeatherData.FirstOrDefault(weather => weather.IdWeather == idWeather);
            if (weatherToDelete != null)
            {
                _context.Remove(weatherToDelete);
                _context.SaveChanges();
                return true;
            }

            // Weather data = FindByID(idWeather);

            return false;
           
        }
    }
    
}

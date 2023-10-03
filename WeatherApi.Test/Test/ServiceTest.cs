using NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherApi.Test.Service;

public class ServiceTest
{
    [Fact]
    public void SaveWeatherSaved()
    {
        // Arrange
        var cityRepository = Substitute.For<ICityRepository>();
        var weatherRepository = Substitute.For<IWeatherRepository>();

        var weatherService = new WeatherService(cityRepositoryMock.Object, weatherRepositoryMock.Object);

        var validWeather = new Weather
        {
            IdCity = "someCityId",
            DayTime = DayTimeEnum.SOL,
            NightTime = NightTimeEnum.NEVE
        };

        // Act
        var result = weatherService.Save(validWeather);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(validWeather, result);

        // Verify that City was retrieved based on IdCity
        cityRepositoryMock.Verify(repo => repo.FindByID(validWeather.IdCity), Times.Once);

        // Verify that Save method was called on WeatherRepository
        weatherRepositoryMock.Verify(repo => repo.Save(validWeather), Times.Once);
    }
}

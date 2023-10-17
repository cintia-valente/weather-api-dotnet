using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestPlatform.Common;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using System.Net;
using System.Text;
using System.Text.Json;
using WeatherApi.Data.DTOs;
using WeatherApi.Entity.Enums;
using WeatherApi.Persistence;

namespace WeatherApi_Integration_Test
{
    public class WeatherControllerTest : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _webApplicationFactory;
        public WeatherControllerTest(WebApplicationFactory<Program> webApplicationFactory) {
            _webApplicationFactory = webApplicationFactory;
        }

        [Fact]
        public async Task PostWeatherTest()
        {
            // Arrange
            var client = _webApplicationFactory.CreateClient();

            var weather = new WeatherRequestDTO
            {
                IdWeather = Guid.NewGuid(),
                Date = DateTime.Now,
                MaxTemperature = 20,
                MinTemperature = 10,
                Precipitation = 30,
                Humidity = 20,
                WindSpeed = 30,
                DayTime = DayTimeEnum.SOL,
                NightTime = NightTimeEnum.NEVE,
                IdCity = Guid.NewGuid(),
                City = new CityResponseDto
                {
                    IdCity = Guid.NewGuid(),
                    Name = "Porto Alegre"
                }
            };

            var requestContent = new StringContent(JsonSerializer.Serialize(weather), Encoding.UTF8, "application/json");

            // Act
            var response = await client.PostAsync("/weather/api/register-weather", requestContent);

            // Assert
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        }
    }
}
using Moq;
using WeatherApi.Models;
using WeatherApi.Models.Enums;
using WeatherApi.Repository.Interfaces;
using WeatherApi.Service;

namespace WeatherApiTest
{
    public class ServiceTest
    {

        private readonly Mock<IWeatherRepository> _weatherRepositoryMock;
        private readonly Mock<ICityRepository> _cityRepositoryMock;
        private readonly WeatherService _weatherService;

        public ServiceTest()
        {
            _weatherRepositoryMock = new Mock<IWeatherRepository>();
            _cityRepositoryMock = new Mock<ICityRepository>();

            _weatherService = new WeatherService(_weatherRepositoryMock.Object, _cityRepositoryMock.Object);
        }

        [Fact(DisplayName = "Dado um objeto Weather, quando salvar o objeto, então chama os métodos FindByID e Save exatamente uma vez.")]
        public void SaveWeatherSaved()
        {
            // Arrange
            var validWeather = new Weather
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
                City = new City
                {
                    IdCity = Guid.NewGuid(),
                    Name = "Porto Alegre"
                }
            };

            _weatherRepositoryMock.Setup(repo => repo.Save(It.IsAny<Weather>()))
           .Returns((Weather w) => w); //Configura (Setup) o comportamento que deve ocorre quando Save for chamado, Save aceita qualquer obj do tipo Weather e retorna um objeto do tipo Weather quando for chamado.

            // Act
            var result = _weatherService.Save(validWeather); //chama o método Save com o mock e armazena o resultado.

            // Assert
            Assert.NotNull(result);
            Assert.Equal(validWeather, result);//verifica se o mock passado como parâmetro é igual ao obj retornado pelo método Save.

            _cityRepositoryMock.Verify(repo => repo.FindByID(It.IsAny<Guid>()), Times.Once); //verifica se FindByID foi chamado exatamente uma vez, passando como parâmetro qualquer argumento do tipo GUID.
            _weatherRepositoryMock.Verify(repo => repo.Save(validWeather), Times.Once); //verifica se Save foi chamado exatamente uma vez, passando como parâmetro o mock criado.

        }

    }
}
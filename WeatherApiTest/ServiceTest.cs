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

        [Fact(DisplayName = "Dado um objeto Weather, quando salvar o objeto Weather, então chama os métodos FindByID e Save exatamente uma vez.")]
        public void SaveWeatherSucess()
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

            _cityRepositoryMock.Verify(repo => repo.FindById(It.IsAny<Guid>()), Times.Once); //verifica se FindByID foi chamado exatamente uma vez, passando como parâmetro qualquer argumento do tipo GUID.
            _weatherRepositoryMock.Verify(repo => repo.Save(validWeather), Times.Once); //verifica se Save foi chamado exatamente uma vez, passando como parâmetro o mock criado.

        }

        [Fact(DisplayName = "Dado um objeto Weather, quando passar valores de enums inválidos, então lança uma exceção.")]
        public void SaveWeatherEnumsError()
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
                DayTime = (DayTimeEnum)10,
                NightTime = (NightTimeEnum).20,
                IdCity = Guid.NewGuid(),
                City = new City
                {
                    IdCity = Guid.NewGuid(),
                    Name = "Porto Alegre"
                }
            };

            _weatherRepositoryMock.Setup(repo => repo.Save(It.IsAny<Weather>()))
            .Throws<ArgumentException>();

            // Act
            var exceptionSave = Assert.Throws<ArgumentException>(() => _weatherService.Save(validWeather));

            // Assert
            Assert.Equal("Valores inválidos para enums DayTime e/ou NightTime.", exceptionSave.Message);
            Assert.Throws<ArgumentException>(() => _weatherService.Save(validWeather));
        }

        [Fact(DisplayName = "Dado um id do Weather, então chama o método FindById exatamente uma vez.")]
        public void FindByIdSucess()
        {
            Guid idWeather = Guid.NewGuid();

            // Arrange
            _weatherRepositoryMock.Setup(repo => repo.FindById(It.IsAny<Guid>()))
            .Returns((Guid id) => new Weather { IdWeather = id });

            // Act
            var result = _weatherService.FindById(idWeather);

            // Assert
            Assert.Equal(idWeather, result.IdWeather);
            _weatherRepositoryMock.Verify(repo => repo.FindById(It.IsAny<Guid>()), Times.Once);
        }

        [Fact(DisplayName = "Dado um id do Weather, quando chamar o método FindById, então lança uma exceção.")]
        public void FindByIdError()
        {
            Guid idWeather = Guid.NewGuid();

            // Arrange
            _weatherRepositoryMock.Setup(repo => repo.FindById(It.IsAny<Guid>()))
            .Throws<Exception>();

            // Act
            var exceptionFindById = Assert.Throws<Exception>(() => _weatherService.FindById(idWeather));

            // Assert
            Assert.Throws<Exception>(() => _weatherService.FindById(idWeather));
        }
    }
}
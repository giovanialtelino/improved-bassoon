using Microsoft.AspNetCore.Mvc;

namespace WebApplication1TestClass.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

        private readonly ILogger<WeatherForecastController> _logger;
        private readonly IConfiguration _configuration;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, IConfiguration conf)
        {
            _logger = logger;
            _configuration = conf;
        }

        [HttpGet("GetWeatherForecast")]
        public IEnumerable<WeatherForecast> Get()
        {
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }

        [HttpGet("GetWeatherForecastExternal")]
        public List<WeatherForecast> GetExternal()
        {
            var url = _configuration["url"];

            var httpClient = new HttpClient();
            var get = httpClient.GetAsync(url + "/testeWeather").Result;

            var result = get.Content.ReadFromJsonAsync<List<WeatherForecast>>().Result;
            return result;
        }
    }
}
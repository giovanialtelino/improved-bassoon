using System.Net;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TestProject1Factory;
using WebApplication1TestClass;
using WireMock;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;
using WireMock.Server;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Hosting;

namespace TestProject1Factory
{
    public class UnitTest1 : IClassFixture<WiremockFactory<Program>>
    {
        private readonly WireMockServer _wiremockServer;
        private readonly WiremockFactory<Program> _program;

        public UnitTest1(WiremockFactory<Program> factory)
        {
            _wiremockServer = factory.Services.GetRequiredService<WireMockServer>();
            _program = factory;
        }


        [Fact]
        public async Task GetSwaggerUI_Returns_OK()
        {
            var application = new UnitTest1(new WiremockFactory<Program>());

            var client = _program.CreateClient();
            var response = await client.GetAsync("/WeatherForecast/GetWeatherForecast");

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task GetSwaggerUI_Returns_OK_External()
        {
            var client = _program.CreateClient();

            var weatherForecastList = new List<WeatherForecast>();
            weatherForecastList.Add(new WeatherForecast { Date = DateTime.Now, Summary = "Test", TemperatureC = 1000 });
            var objectAsJson = JsonSerializer.Serialize<List<WeatherForecast>>(weatherForecastList);


            _wiremockServer
   .Given(Request.Create().WithPath("/testeWeather").UsingGet())
   .RespondWith(
     Response.Create()
       .WithStatusCode(200)
       .WithBody(objectAsJson)
   );


            var response = await client.GetAsync("/WeatherForecast/GetWeatherForecastExternal");

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
    }
}

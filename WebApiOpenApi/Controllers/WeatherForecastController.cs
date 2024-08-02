using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;

namespace WebApiOpenApi.Controllers
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

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        [EndpointSummary("This is a summary from OpenApi attributes.")]
        [EndpointDescription("This is a description from OpenApi attributes.")]
        [Produces(typeof(IEnumerable<WeatherForecast>))]
        [HttpGet(Name = "GetWeatherForecast")]
        public IEnumerable<WeatherForecast> Get()
        {
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }

        [EndpointSummary("This is a second summary from OpenApi attributes.")]
        [EndpointDescription("This is a second description from OpenApi attributes.")]
        [Produces(typeof(IEnumerable<WeatherForecast>))]
        [HttpGet(Name = "GetWeatherForecastWithParameter")]
        public IEnumerable<WeatherForecast> GetWithParameter(
            [Description("parameter name description using OpenApi")] string name)
        {
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }
        
    }
}

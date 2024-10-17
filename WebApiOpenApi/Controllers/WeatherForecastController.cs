using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;

namespace WebApiOpenApi.Controllers;

[Authorize(AuthenticationSchemes = "Bearer")]
[ApiController]
[Route("api/[controller]")]
public class WeatherForecastController(ILogger<WeatherForecastController> _logger) : ControllerBase
{
    private static readonly string[] Summaries =
    [
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    ];

    [AllowAnonymous]
    [EndpointSummary("This is a summary from OpenApi attributes.")]
    [EndpointDescription("This is a description from OpenApi attributes.")]
    [Produces(typeof(IEnumerable<WeatherForecast>))]
    [HttpGet("GetWeatherForecast")]
    public IActionResult Get()
    {
        _logger.LogDebug("GetWeatherForecast with OpenAPI definitions");

        return Ok(Enumerable.Range(1, 5).Select(index => new WeatherForecast
        {
            Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            TemperatureC = Random.Shared.Next(-20, 55),
            Summary = Summaries[Random.Shared.Next(Summaries.Length)]
        })
        .ToArray());
    }

    [EndpointSummary("This is a second summary from OpenApi attributes.")]
    [EndpointDescription("This is a second description from OpenApi attributes.")]
    [Produces(typeof(IEnumerable<WeatherForecast>))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [HttpGet("GetWeatherForecastWithParameter")]
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

    [EndpointSummary("This is a second summary from OpenApi attributes.")]
    [EndpointDescription("This is a second description from OpenApi attributes.")]
    [HttpPut("PutWeatherForecast")]
    public IActionResult PutWeatherForecast(
        [Description("parameter put item using OpenApi")] WeatherForecast weatherForecast)
    {
        return Created();
    }

    [EndpointSummary("This is a second summary from OpenApi attributes.")]
    [EndpointDescription("This is a second description from OpenApi attributes.")]
    [Produces(typeof(IEnumerable<WeatherForecast>))]
    [HttpPost("PostWeatherForecast")]
    public IActionResult PostWeatherForecast(
        [Description("parameter post item using OpenApi")] WeatherForecast weatherForecast)
    {
        return Ok(weatherForecast);
    }
}

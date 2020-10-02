using Microsoft.AspNet.OData;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DotNet5Exploratory.Controllers
{    
    [Route("odata/[controller]")]    
    [ApiExplorerSettings(IgnoreApi = false)]
    public class WeatherForecastController : ODataController
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

        [HttpGet]
        [EnableQuery]
        public IEnumerable<WeatherForecast> Get()
        {
            var rng = new Random();
            var wf = Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            })
            .ToArray();
            return wf;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] WeatherForecast wf)
        {
            return (IActionResult)base.Ok();
        }

        [HttpPatch]
        public async Task<IActionResult> Patch([FromBody] WeatherForecast wf)
        {
            return (IActionResult)base.Ok();
            //}

        }
    }
}

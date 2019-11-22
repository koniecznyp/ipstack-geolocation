using System.Threading.Tasks;
using Commands;
using Microsoft.AspNetCore.Mvc;
using Services.Interfaces;

namespace Service.Geolocation.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class GeolocationController : ControllerBase
    {
        private readonly IGeolocationService _geolocationService;
        public GeolocationController(IGeolocationService geolocationService)
        {
            _geolocationService = geolocationService;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CreateGeolocation command) 
        {
            await _geolocationService.AddAsync(command.Address);
            return Created($"geolocation/{command.Address}", null);
        }

        [HttpGet("{address}")]
        public async Task<IActionResult> Get(string address)
        {
            return Ok(await _geolocationService.GetAsync(address));
        }

        [HttpDelete]
        public async Task<IActionResult> Delete([FromQuery] DeleteGeolocation command)
        {
            await _geolocationService.DeleteAsync(command.Address);
            return Accepted();
        }
    }
}

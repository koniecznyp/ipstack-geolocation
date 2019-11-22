using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Commands;
using Dto;
using FluentAssertions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Moq;
using Newtonsoft.Json;
using Service.Geolocation.Api;
using Service.Geolocation.Api.Controllers;
using Services.Interfaces;
using Xunit;

namespace Controllers
{
    public class GeolocationControllerTests
    {
        protected readonly TestServer Server;
        protected readonly HttpClient Client;

        public GeolocationControllerTests()
        {
            Server = new TestServer(new WebHostBuilder()
                    .UseEnvironment("Development")
                    .UseConfiguration(new ConfigurationBuilder()
                        .AddJsonFile("appsettings.json")
                        .Build()
                    ).UseStartup<Startup>());
            Client = Server.CreateClient();
        }

        [Fact]
        public async Task Post_GeolocationDoesNotExists_ShouldBeCreated()
        {
            var command = new CreateGeolocation() { Address = "8.8.8.8" };
            var geolocationServiceMock = new Mock<IGeolocationService>();

            var geolocationController = new GeolocationController(geolocationServiceMock.Object);

            var payload = GetPayload(command);
            var response = await Client.PostAsync("geolocation", payload);
            response.StatusCode.Should().BeEquivalentTo(HttpStatusCode.Created);
            response.Headers.Location.ToString().Should().BeEquivalentTo($"geolocation/{command.Address}");

            var details = await GetAsync(command.Address);
            details.Ip.Should().Be(command.Address);
            details.City.Should().Be("Mountain View");
            details.Longitude.Should().Be(-122.07540893554688d);
            details.Latitude.Should().Be(37.419158935546875d);
        }

        private async Task<GeolocationDto> GetAsync(string address)
        {
            var response = await Client.GetAsync($"geolocation/{address}");
            var responseString = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<GeolocationDto>(responseString);
        }

        private StringContent GetPayload(object data)
        {
            var json = JsonConvert.SerializeObject(data);
            return new StringContent(json, Encoding.UTF8, "application/json");
        }           
    }
}
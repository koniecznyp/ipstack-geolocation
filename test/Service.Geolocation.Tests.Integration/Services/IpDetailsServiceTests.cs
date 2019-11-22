using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Configuration;
using FluentAssertions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Moq;
using Services;
using Xunit;
using Domain;
using Services.Exceptions;
using System.Net;

namespace Service.Geolocation.Tests.Integration.Services
{
    public class IpDetailsServiceTests
    {
        private readonly IOptions<IpDetailsServiceSettings> _options;

        public IpDetailsServiceTests()
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", false)
                .Build();
            _options = Options.Create(configuration.GetSection("ipstack").Get<IpDetailsServiceSettings>());
        }

        [Fact]
        public async Task GetDetailsAboutAddress_OptionsAreCorrect_ShouldReturnsGeolocationData()
        {
            string address = "8.8.8.8";
            var ipDetailsService = new IpDetailsService(_options);

            var result = await ipDetailsService.GetDetailsAboutAddress(address);

            result.Should().NotBeNull();
            result.Ip.Should().Be("8.8.8.8");
            result.City.Should().Be("Mountain View");
            result.Latitude.Should().Be(37.419158935546875d);
            result.Longitude.Should().Be(-122.07540893554688d);
        }

        [Fact]
        public async Task GetDetailsAboutAddress_AccessKeyIsWrong_ThrowsException()
        {
            string address = "8.8.8.8";
            var optionsMock = new Mock<IOptions<IpDetailsServiceSettings>>();
            optionsMock.Setup(x => x.Value)
                .Returns(new IpDetailsServiceSettings()
                {
                    Address = _options.Value.Address,
                    AccessKey = "blah"
                });

            var ipDetailsService = new IpDetailsService(optionsMock.Object);

            var exception = await Assert.ThrowsAsync<ServiceException>(() => 
                ipDetailsService.GetDetailsAboutAddress(address));

            exception.Code.Should().Be(HttpStatusCode.InternalServerError);
        }

        [Fact]
        public async Task GetDetailsAboutAddress_ServiceIsUnavailable_ThrowsException()
        {
            string address = "8.8.8.8";
            var optionsMock = new Mock<IOptions<IpDetailsServiceSettings>>();
            optionsMock.Setup(x => x.Value)
                .Returns(new IpDetailsServiceSettings()
                {
                    Address = "http://api.some.bad.address.ipstack.com/",
                    AccessKey = _options.Value.AccessKey
                });

            var ipDetailsService = new IpDetailsService(optionsMock.Object);

            var exception = await Assert.ThrowsAsync<ServiceException>(() => 
                ipDetailsService.GetDetailsAboutAddress(address));

            exception.Code.Should().Be(HttpStatusCode.ServiceUnavailable);
        }
    }
}
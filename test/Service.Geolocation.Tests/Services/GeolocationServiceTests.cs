using System.Net;
using System.Threading.Tasks;
using Dto;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Network.Interfaces;
using Repositories.Interfaces;
using Services;
using Services.Exceptions;
using Services.Interfaces;
using Xunit;

namespace Service.Geolocation.Tests
{
    public class GeolocationServiceTests
    {
        [Fact]
        public async Task AddAsync_GeolocationDoesNotExists_ShouldBeCreated()
        {
            var ipAddress = "192.168.0.1";
            var geolocationRepositoryMock = new Mock<IGeolocationRepository>();
            geolocationRepositoryMock.Setup(x => x.GetAsync(It.IsAny<string>()))
                .ReturnsAsync(value: null);
            var ipDetailsServiceMock = new Mock<IIpDetailsService>();
            ipDetailsServiceMock.Setup(x => x.GetDetailsAboutAddress(It.IsAny<string>()))
                .ReturnsAsync(new Domain.Geolocation() { Ip = ipAddress });
            var networkAddressMock = new Mock<INetworkAddress>();

            var geolocationService = new GeolocationService(geolocationRepositoryMock.Object, ipDetailsServiceMock.Object,
                networkAddressMock.Object);

            await geolocationService.AddAsync(ipAddress);

            geolocationRepositoryMock.Verify(x => x.AddAsync(It.IsAny<Domain.Geolocation>()), Times.Once);
            ipDetailsServiceMock.Verify(x => x.GetDetailsAboutAddress(It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public async Task AddAsync_GeolocationAlreadyExists_ThrowsException()
        {
            var ipAddress = "192.168.0.1";
            var geolocationRepositoryMock = new Mock<IGeolocationRepository>();
            geolocationRepositoryMock.Setup(x => x.GetAsync(It.IsAny<string>()))
                .ReturnsAsync(new Domain.Geolocation());
            var ipDetailsServiceMock = new Mock<IIpDetailsService>();
            var networkAddressMock = new Mock<INetworkAddress>();

            var geolocationService = new GeolocationService(geolocationRepositoryMock.Object, ipDetailsServiceMock.Object,
                networkAddressMock.Object);

            var exception = await Assert.ThrowsAsync<ServiceException>(() => 
                geolocationService.AddAsync(ipAddress));

            exception.Code.Should().Be(HttpStatusCode.Conflict);
            geolocationRepositoryMock.Verify(x => x.AddAsync(It.IsAny<Domain.Geolocation>()), Times.Never);
            ipDetailsServiceMock.Verify(x => x.GetDetailsAboutAddress(It.IsAny<string>()), Times.Never);
        }

        [Fact]
        public async Task GetAsync_GeolocationDoesNotExists_ThrowsException()
        {
            var ipAddress = "192.168.0.1";
            var geolocationRepositoryMock = new Mock<IGeolocationRepository>();
            geolocationRepositoryMock.Setup(x => x.GetAsync(It.IsAny<string>()))
                .ReturnsAsync(value: null);
            var ipDetailsServiceMock = new Mock<IIpDetailsService>();
            var networkAddressMock = new Mock<INetworkAddress>();

            var geolocationService = new GeolocationService(geolocationRepositoryMock.Object, ipDetailsServiceMock.Object,
                networkAddressMock.Object);

            var exception = await Assert.ThrowsAsync<ServiceException>(() => 
                geolocationService.GetAsync(ipAddress));

            exception.Code.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task GetAsync_GeolocationExists_ShouldBeReturned()
        {
            var ipAddress = "192.168.0.1";
            var geolocationRepositoryMock = new Mock<IGeolocationRepository>();
            geolocationRepositoryMock.Setup(x => x.GetAsync(It.IsAny<string>()))
                .ReturnsAsync(new Domain.Geolocation(){
                    City = "San Francisco",
                    Latitude = 37.773972d,
                    Longitude = -122.431297d
                });
            var ipDetailsServiceMock = new Mock<IIpDetailsService>();
            var networkAddressMock = new Mock<INetworkAddress>();

            var geolocationService = new GeolocationService(geolocationRepositoryMock.Object, ipDetailsServiceMock.Object,
                networkAddressMock.Object);

            var result = await geolocationService.GetAsync(ipAddress);

            result.Should().NotBeNull().And.BeOfType(typeof(GeolocationDto));
            result.City.Should().Be("San Francisco");
            result.Latitude.Should().Be(37.773972d);
            result.Longitude.Should().Be(-122.431297d);
        }
    }
}
using System;
using System.Net;
using System.Threading.Tasks;
using Configuration;
using Domain;
using Dto;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Network.Interfaces;
using Repositories.Interfaces;
using Services.Exceptions;
using Services.Interfaces;

namespace Services
{
    public class GeolocationService : IGeolocationService
    {
        private readonly IGeolocationRepository _geolocationRepository;
        private readonly IIpDetailsService _service;
        private readonly INetworkAddress _networkAddress;
        
        public GeolocationService(IGeolocationRepository geolocationRepository, IIpDetailsService service,
            INetworkAddress networkAddress)
        {
            _geolocationRepository = geolocationRepository;
            _service = service;
            _networkAddress = networkAddress;
        }

        public async Task AddAsync(string address)
        {
            _networkAddress.CheckIfAddressIsValid(address);
            var geolocation = await _geolocationRepository.GetAsync(address);
            if(geolocation != null)
            {
                throw new ServiceException(HttpStatusCode.Conflict, 
                    $"Geolocation with address {address} already exists");
            }
            var newGeolocation = await _service.GetDetailsAboutAddress(address);
            newGeolocation.Key = address;
            await _geolocationRepository.AddAsync(newGeolocation);
        }

        public async Task DeleteAsync(string address)
        {
            _networkAddress.CheckIfAddressIsValid(address);
            var geolocation = await GetOrFailAsync(address);
            await _geolocationRepository.DeleteAsync(geolocation);
        }

        public async Task<GeolocationDto> GetAsync(string address)
        {
            _networkAddress.CheckIfAddressIsValid(address);
            var geolocation = await GetOrFailAsync(address);
            return new GeolocationDto() { // whatever you want to return
                Ip = geolocation.Ip,
                City = geolocation.City,
                Longitude = geolocation.Longitude,
                Latitude = geolocation.Latitude
            };
        }

        private async Task<Geolocation> GetOrFailAsync(string ipAddress)
        {
            var geolocation = await _geolocationRepository.GetAsync(ipAddress);
            if(geolocation is null)
            {
                throw new ServiceException(HttpStatusCode.NotFound, 
                    $"Geolocation with address {ipAddress} not exists");
            }
            return geolocation;
        }
    }
}
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Configuration;
using Domain;
using Domain.Errors;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RestSharp;
using RestSharp.Deserializers;
using RestSharp.Serialization.Json;
using Services.Exceptions;
using Services.Interfaces;

namespace Services
{
    public class IpDetailsService : IIpDetailsService
    {
        private readonly IpDetailsServiceSettings _settings;

        public IpDetailsService(IOptions<IpDetailsServiceSettings> settings)
        {
            _settings = settings.Value;
        }

        public async Task<Geolocation> GetDetailsAboutAddress(string address)
        {
            var request = new RestRequest("{address}");
            request.AddUrlSegment("address", address);
            request.AddParameter("access_key", _settings.AccessKey);
            return await ExecuteAsync<Geolocation>(request);
        }

        public async Task<T> ExecuteAsync<T>(RestRequest request) where T : new()
        {
            var client = new RestClient(_settings.Address);
            var response = await client.ExecuteTaskAsync<T>(request);

            if (response.ErrorException != null)
            {
                throw new ServiceException(HttpStatusCode.ServiceUnavailable,
                    $"An error occurred while processing the request [Details: external service is unavailable]");
            }

            IDeserializer deserializer = new JsonDeserializer();
            ErrorResponse error = deserializer.Deserialize<ErrorResponse>(response);
            if (error.Success == false)
            {
                throw new ServiceException(HttpStatusCode.InternalServerError,
                    $"There was a problem processing your request with an external provider [Details: {error.Error.Info}]");
            }
            return response.Data;
        }
    }
}
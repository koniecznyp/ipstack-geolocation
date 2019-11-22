using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain;
using Repositories.Interfaces;

namespace Repositories
{
    public class InMemoryGeolocationRepository : IGeolocationRepository
    {
        private static readonly ISet<Geolocation> _geolocations = new HashSet<Geolocation>();

        public async Task AddAsync(Geolocation geolocation)
        {
            _geolocations.Add(geolocation);
			await Task.CompletedTask;
        }

        public async Task DeleteAsync(Geolocation geolocation)
        {
            var obj = await GetAsync(geolocation.Key);
			_geolocations.Remove(obj);
			await Task.CompletedTask;
        }

        public async Task<Geolocation> GetAsync(string key)
            => await Task.FromResult(_geolocations.SingleOrDefault(x => x.Key == key));
    }
}
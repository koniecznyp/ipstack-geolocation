using System.Threading.Tasks;
using Domain;

namespace Repositories.Interfaces
{
    public interface IGeolocationRepository
    {
        Task AddAsync(Geolocation geolocation);
        Task<Geolocation> GetAsync(string key);
        Task DeleteAsync(Geolocation geolocation);
    }
}
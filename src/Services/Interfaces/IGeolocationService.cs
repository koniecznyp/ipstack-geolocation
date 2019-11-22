using System.Threading.Tasks;
using Dto;

namespace Services.Interfaces
{
    public interface IGeolocationService
    {
        Task AddAsync(string address);
        Task<GeolocationDto> GetAsync(string address);
        Task DeleteAsync(string address);
    }
}
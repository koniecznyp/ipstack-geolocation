using System.Threading.Tasks;
using Domain;

namespace Services.Interfaces
{
    public interface IIpDetailsService
    {
        Task<Geolocation> GetDetailsAboutAddress(string address);
    }
}
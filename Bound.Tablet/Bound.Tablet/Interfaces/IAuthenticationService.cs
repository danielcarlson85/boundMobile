using System.Threading.Tasks;
using Bound.Tablet.Dots.Authentication;

namespace Bound.Tablet.Services.Interfaces
{
    public interface IAuthenticationService
    {
        Task<CleanADUserResponse> AuthenticationAsync(string username, string password);
    }
}

using System.Threading.Tasks;
using AtHomeProject.Domain.Models;
using AtHomeProject.Domain.Models.Auth;

namespace AtHomeProject.Domain.Interfaces
{
    public interface IAuthService
    {
        Task<AuthResponse> AuthenticateAsync(UserModel model);
    }
}

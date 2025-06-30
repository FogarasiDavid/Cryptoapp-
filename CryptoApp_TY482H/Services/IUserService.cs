using System.Threading.Tasks;
using CryptoApp_TY482H.DTOs.Auth;

namespace CryptoApp_TY482H.Services.Interfaces
{
    public interface IUserService
    {
        Task<UserResponseDto> RegisterAsync(RegisterUserDto dto);
        Task<string> LoginAsync(string username, string password);
    }
}
using Unity.Monitoring.Models;

namespace Unity.Monitoring.Services
{
    public interface IUserService
    {
        Task<User?> AuthenticateUser(UserLoginDto loginDto);
        Task<UserDto> CreateUser(UserLoginDto registerDto);
    }
}

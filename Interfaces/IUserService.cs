using Unity.Monitoring.Models;

namespace Unity.Monitoring.Services
{
    public interface IUserService
    {
        Task<UserDto?> AuthenticateUser(UserLoginDto loginDto);
        Task CreateUser(UserLoginDto registerDto);
        Task UpdateUserRole(UserUpdateDto updateDto);
    }
}

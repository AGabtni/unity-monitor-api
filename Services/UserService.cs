using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Unity.Monitoring.Data;
using Unity.Monitoring.Models;

namespace Unity.Monitoring.Services
{
    public class UserService : IUserService
    {
        private readonly AppDbContext _dbContext;
        private readonly IMapper _mapper;

        public UserService(AppDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<UserDto?> AuthenticateUser(UserLoginDto loginDto)
        {
            // Check if user exists
            var user = await _dbContext.User.FirstOrDefaultAsync(u =>
                u.Username == loginDto.Username
            );

            // Validate pwd against hash
            if (user == null || !BCrypt.Net.BCrypt.Verify(loginDto.Password, user.PasswordHash))
                return null;

            // Log login timestap, update tabke
            user.LastLoginUtc = DateTime.UtcNow;
            await _dbContext.SaveChangesAsync();

            return _mapper.Map<UserDto>(user);
        }

        public async Task CreateUser(UserLoginDto registerDto)
        {
            // Check if user exists
            if (await _dbContext.User.AnyAsync(u => u.Username == registerDto.Username))
                throw new ArgumentException("Username already taken");

            var user = _mapper.Map<User>(registerDto);

            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(registerDto.Password);
            user.Role = "User";
            user.CreatedUtc = DateTime.UtcNow;
            //user.LastLoginUtc = DateTime.UtcNow; // Assuming we make them login instantly after registerign

            _dbContext.User.Add(user);
            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateUserRole(UserUpdateDto updateDto)
        {
            if (string.IsNullOrWhiteSpace(updateDto.Role))
                throw new ArgumentException($"Invalid role {updateDto.Username}");

            // Check if user exists
            var user = await _dbContext.User.FirstOrDefaultAsync(u =>
                u.Username == updateDto.Username
            );
            if (user == null)
                throw new ArgumentException($"User with {updateDto.Username} does not exist");

            user.Role = updateDto.Role;
            await _dbContext.SaveChangesAsync();
        }
    }
}

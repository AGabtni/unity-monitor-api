using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Unity.Monitoring.Models;
using Unity.Monitoring.Services;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly IJwtService _jwtService;

    public AuthController(IUserService userService, IJwtService jwtService)
    {
        _userService = userService;
        _jwtService = jwtService;
    }

    [HttpPost("login")]
    public async Task<ActionResult<UserDto>> Login(UserLoginDto login)
    {
        var user = await _userService.AuthenticateUser(login);
        if (user == null)
            return Unauthorized("Invalid credentials");

        //  and return token
        try
        {
            user.Jwt = _jwtService.GenerateJwtToken(user);
            return Ok(user);
        }
        catch (ArgumentException error)
        {
            return BadRequest(error.Message);
        }
    }

    [HttpPost("register")]
    public async Task<IStatusCodeActionResult> Register(UserLoginDto register)
    {
        try
        {
            // Return user dto on success
            await _userService.CreateUser(register);
            return Ok($"User '{register.Username}' created successfully");
        }
        catch (ArgumentException error)
        {
            return BadRequest(error.Message);
        }
    }

    [Authorize(Policy = "AdminRights")]
    [HttpPut("update-role")]
    public async Task<ActionResult> UpdateRole(
        [FromBody] UserUpdateDto update,
        [FromHeader(Name = "Authorization")] string? authHeader
    )
    {
        try
        {
            // Extract token from bearer
            var token = authHeader?.Split(' ').LastOrDefault();

            if (!string.IsNullOrEmpty(token))
            {
                var handler = new JwtSecurityTokenHandler();
                var jwt = handler.ReadJwtToken(token);

                if (jwt.Subject.Equals(update.Username, StringComparison.OrdinalIgnoreCase))
                    throw new UnauthorizedAccessException("You cannot change your own role");
            }

            await _userService.UpdateUserRole(update);
            return Ok($"User {update.Username} is has now role {update.Role}");
        }
        catch (ArgumentException error)
        {
            return BadRequest(error.Message);
        }
    }
}

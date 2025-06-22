using System.IdentityModel.Tokens.Jwt;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
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
    public async Task<ActionResult<string>> Login(UserLoginDto login)
    {
        var user = await _userService.AuthenticateUser(login);
        if (user == null)
            return Unauthorized("Invalid credentials");

        //  and return token
        try
        {
            var token = _jwtService.GenerateJwtToken(user);
            return Ok(token);
        }
        catch (ArgumentException error)
        {
            return BadRequest(error.Message);
        }
    }

    [HttpPost("register")]
    public async Task<ActionResult<UserDto>> Register(UserLoginDto register)
    {
        try
        {
            // Return user dto on success
            var newUser = await _userService.CreateUser(register);
            return Created("", newUser);
        }
        catch (ArgumentException error)
        {
            return BadRequest(error.Message);
        }
    }


}

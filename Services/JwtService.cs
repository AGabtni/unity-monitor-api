using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Unity.Monitoring.Models;

namespace Unity.Monitoring.Services
{
    public class JwtService : IJwtService
    {
        private readonly IConfiguration _config;
        private readonly string _jwtKey;

        public JwtService(IConfiguration config)
        {
            _config = config;
            _jwtKey =
                Environment.GetEnvironmentVariable("PGSECRET")
                ?? throw new InvalidOperationException(
                    "JWT Key is missing. Set 'PGSECRET' as environment variable"
                );

            ValidateJwtKey(_jwtKey);
        }

        private void ValidateJwtKey(string key)
        {
            if (string.IsNullOrWhiteSpace(key))
                throw new ArgumentException("JWT Key cannot be empty");

            var byteCount = Encoding.UTF8.GetByteCount(key);
            if (byteCount < 32)
                throw new ArgumentException($"JWT Key must be at least 32 bytes long");
        }

        public void ConfigureJwtAuthentication(IServiceCollection services)
        {
            services
                .AddAuthentication("Bearer")
                .AddJwtBearer(
                    "Bearer",
                    options =>
                    {
                        options.TokenValidationParameters = new TokenValidationParameters
                        {
                            ValidateIssuer = true,
                            ValidateAudience = true,
                            ValidateLifetime = true,
                            ValidateIssuerSigningKey = true,
                            ValidIssuer = _config["Jwt:Issuer"] ?? "http://auth.unity-monitor.com",
                            ValidAudience =
                                _config["Jwt:Audience"] ?? "http://www.unity-monitor.com",
                            IssuerSigningKey = new SymmetricSecurityKey(
                                Encoding.UTF8.GetBytes(_jwtKey)
                            ),
                        };
                    }
                );
        }

        public string GenerateJwtToken(User user)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            if (string.IsNullOrWhiteSpace(user.Username))
                throw new ArgumentException("Username cannot be empty");

            if (string.IsNullOrWhiteSpace(user.Role))
                throw new ArgumentException("Role cannot be empty");

            var jwtExpiration = _config["Jwt:ExpireMinutes"] ?? "60";

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Username),
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Role, user.Role),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            };

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"] ?? "http://auth.unity-monitor.com",
                audience: _config["Jwt:Audience"] ?? "http://www.unity-monitor.com",
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(double.Parse(jwtExpiration)),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}

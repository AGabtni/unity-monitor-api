using Unity.Monitoring.Models;

namespace Unity.Monitoring.Services
{
    public interface IJwtService
    {
        void ConfigureJwtAuthentication(IServiceCollection services);
        string GenerateJwtToken(User user);
    }
}

using System.ComponentModel.DataAnnotations;

namespace Unity.Monitoring.Models
{
    public class UserDto
    {
        public int Id { get; set; }

        [Required]
        public required string Username { get; set; }

        [Required]
        [AllowedRoles]
        public required string Role { get; set; }

        [Required]
        public required string Jwt { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;

namespace Unity.Monitoring.Models
{
    public class UserUpdateDto
    {
        [Required]
        public required string Username { get; set; }

        [Required]
        [AllowedRoles]
        public required string Role { get; set; }
    }
}

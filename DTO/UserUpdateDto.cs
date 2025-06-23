using System.ComponentModel.DataAnnotations;

namespace Unity.Monitoring.Models
{
    public class UserUpdateDto
    {
        [Required]
        public required string Username { get; set; }

        [Required]
        [AllowedRoles("Admin", "User")]
        public required string Role { get; set; }
    }
}

public class AllowedRolesAttribute : ValidationAttribute
{
    private readonly string[] _allowedRoles;

    public AllowedRolesAttribute(params string[] allowedRoles)
    {
        _allowedRoles = allowedRoles;
    }

    protected override ValidationResult IsValid(object value, ValidationContext context)
    {
        if (value is string role && _allowedRoles.Contains(role))
            return ValidationResult.Success;

        return new ValidationResult($"Role must be one of: {string.Join(", ", _allowedRoles)}");
    }
}

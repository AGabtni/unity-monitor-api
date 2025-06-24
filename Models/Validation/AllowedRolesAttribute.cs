using System.ComponentModel.DataAnnotations;

namespace Unity.Monitoring.Models
{
    public static class UserRoles
    {
        public const string Admin = "Admin";
        public const string User = "User";

        public static readonly string[] Roles = { Admin, User };
    }

    public class AllowedRolesAttribute : ValidationAttribute
    {
        private readonly string[] _allowedRoles;

        public AllowedRolesAttribute()
        {
            _allowedRoles = UserRoles.Roles;
        }

        protected override ValidationResult IsValid(object value, ValidationContext context)
        {
            if (value is string role && _allowedRoles.Contains(role))
                return ValidationResult.Success;

            return new ValidationResult($"Role must be one of: {string.Join(", ", _allowedRoles)}");
        }
    }
}

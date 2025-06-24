using System.ComponentModel.DataAnnotations;

namespace Unity.Monitoring.Models
{
    public static class AssetStatuses
    {
        public const string Active = "Active";
        public const string Inactive = "Inactive";
        public const string Maintenance = "Maintenance";
        public const string Decomissioned = "Decomissioned";

        public static readonly string[] Status = { Active, Inactive, Maintenance, Decomissioned };
    }

    public class AllowedAssetStatusAttribute : ValidationAttribute
    {
        private readonly string[] _allowedAssetStatus;

        public AllowedAssetStatusAttribute()
        {
            _allowedAssetStatus = AssetStatuses.Status;
        }

        protected override ValidationResult IsValid(object value, ValidationContext context)
        {
            if (value is string status && _allowedAssetStatus.Contains(status))
                return ValidationResult.Success;

            return new ValidationResult(
                $"Asset status must be one of: {string.Join(", ", _allowedAssetStatus)}"
            );
        }
    }
}

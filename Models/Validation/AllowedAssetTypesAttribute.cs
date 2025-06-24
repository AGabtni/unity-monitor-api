using System.ComponentModel.DataAnnotations;

namespace Unity.Monitoring.Models
{
    public static class EnergyTypes
    {
        public const string Hydro = "Hydro";
        public const string Solar = "Solar";
        public const string Wind = "Wind";
        public const string Other = "Other";

        public static readonly string[] Types = { Hydro, Solar, Wind, Other };
    }

    public class AllowedAssetTypesAttribute : ValidationAttribute
    {
        private readonly string[] _allowedAssetTypes;

        public AllowedAssetTypesAttribute()
        {
            _allowedAssetTypes = EnergyTypes.Types;
        }

        protected override ValidationResult IsValid(object value, ValidationContext context)
        {
            if (value is string type && _allowedAssetTypes.Contains(type))
                return ValidationResult.Success;

            return new ValidationResult(
                $"Asset type must be one of: {string.Join(", ", _allowedAssetTypes)}"
            );
        }
    }
}

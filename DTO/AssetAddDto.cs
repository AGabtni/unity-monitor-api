using System.ComponentModel.DataAnnotations;
using Unity.Monitoring.Models;

namespace Unity.Monitoring.DTO
{
    public class AssetAddDto
    {
        [Required]
        public required string Name { get; set; }

        [Required]
        [AllowedAssetTypes]
        public required string Type { get; set; }

        [Required]
        [AllowedAssetStatus]
        public required string Status { get; set; }
    }
}

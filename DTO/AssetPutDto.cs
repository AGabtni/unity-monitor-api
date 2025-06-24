using System.ComponentModel.DataAnnotations;
using Unity.Monitoring.Models;

namespace Unity.Monitoring.DTO
{
    public class AssetPutDto
    {
        public string? Name { get; set; }

        [Required]
        [AllowedAssetStatus]
        public required string Status { get; set; }
    }
}

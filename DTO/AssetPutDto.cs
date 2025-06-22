using System.ComponentModel.DataAnnotations;
using Unity.Monitoring.Models;

namespace Unity.Monitoring.DTO
{
    public class AssetPutDto
    {
        [Required]
        public required string Name { get; set; }

        [Required]
        public required AssetStatus Status { get; set; }
    }
}

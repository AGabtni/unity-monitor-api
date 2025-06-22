using System.ComponentModel.DataAnnotations;
using Unity.Monitoring.Models;

namespace Unity.Monitoring.DTO
{
    public class AssetAddDto
    {
        [Required]
        public required string Name { get; set; }

        [Required]
        public required EnergyType Type { get; set; }

        [Required]
        public AssetStatus Status { get; set; }
    }
}

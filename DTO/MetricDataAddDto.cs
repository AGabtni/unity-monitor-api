using System.ComponentModel.DataAnnotations;

namespace Unity.Monitoring.DTO
{
    public class MetricDataAddDto
    {
        [Required]
        public required int AssetId { get; set; }
        public double PowerOutput { get; set; }
        public double EnergyProduced { get; set; }
        public double TotalAvailableHours { get; set; }
    }
}

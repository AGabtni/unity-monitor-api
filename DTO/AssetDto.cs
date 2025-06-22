using Unity.Monitoring.Models;

namespace Unity.Monitoring.DTO
{
    public class AssetDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public EnergyType Type { get; set; }
        public AssetStatus Status { get; set; }
        public List<MetricDataDto> Metrics { get; set; }
    }
}

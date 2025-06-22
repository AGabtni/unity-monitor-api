using System.ComponentModel.DataAnnotations;

namespace Unity.Monitoring.Models
{
    public enum EnergyType
    {
        Hydro,
        Solar,
        Wind,
        Other,
    }

    public enum AssetStatus
    {
        Active,
        Inactive,
        Maintenance,
        Decomissioned,
    }

    public class Asset
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public required EnergyType Type { get; set; }
        public required AssetStatus Status { get; set; }
        public ICollection<MetricData> Metrics { get; set; } = new List<MetricData>();
    }
}

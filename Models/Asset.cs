namespace Unity.Monitoring.Models
{
    public enum EnergyType
    {
        Hydro,
        Solar,
        Wind,
        Other
    }

    public enum AssetStatus
    {
        Active,
        Inactive,
        Maintenance,
        Decomissioned
    }

    public class Asset
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public EnergyType Type { get; set; }
        public AssetStatus Status { get; set; }
        public ICollection<MetricData> Metrics { get; set; }
    }
}
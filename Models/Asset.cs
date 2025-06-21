namespace Unity.Monitoring.Models
{
    public class Asset
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public string Status { get; set; }
        public ICollection<MetricData> Metrics { get; set; }
    }
}
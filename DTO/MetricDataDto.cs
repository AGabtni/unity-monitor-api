namespace Unity.Monitoring.DTO
{
    public class MetricDataDto
    {
        public int Id { get; set; }
        public DateTime Timestamp { get; set; }
        public double PowerOutput { get; set; }
        public double EnergyProduced { get; set; }
        public double TotalAvailableHours { get; set; }
    }
}

namespace Unity.Monitoring.Models
{
    public class MetricData
    {
        public int Id { get; set; }
        public int AssetId { get; set; }
        public Asset Asset { get; set; } 
        public DateTime Timestamp { get; set; }

        // Tracked metrics (Common for hydro/solar/wind energy)
        public double PowerOutput { get; set; } //Energy in kW being produced at timesamp
        public double EnergyProduced { get; set; } // Cumulative energy produced (or total energy in kW produced up to timesamp)
        public double TotalAvailableHours { get; set; } // Cumulative time the asset has been available in hours 

    }
}
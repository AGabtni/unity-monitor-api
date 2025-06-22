using Unity.Monitoring.DTO;
using Unity.Monitoring.Models;

namespace Unity.Monitoring.Services
{
    public interface IMetricDataService
    {
        Task<MetricDataDto> CreateAsync(MetricDataAddDto dto);
        Task<MetricDataDto> GetByIdAsync(int id);
        Task<IEnumerable<MetricDataDto>> GetAllAssetMetricsAsync(int assetId);
        Task<IEnumerable<MetricDataDto>> GetMetricsByDateRangeAsync(
            int assetId,
            DateTime from,
            DateTime to
        );
    }
}

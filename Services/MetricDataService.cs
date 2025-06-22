using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Unity.Monitoring.Data;
using Unity.Monitoring.DTO;
using Unity.Monitoring.Models;

namespace Unity.Monitoring.Services
{
    public class MetricDataService : IMetricDataService
    {
        private readonly AppDbContext _dbContext;
        private readonly IMapper _mapper;

        public MetricDataService(AppDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<MetricDataDto> CreateAsync(MetricDataAddDto dto)
        {
            // Validate AssetId
            if (!await AssetExistsAsync(dto.AssetId))
                throw new ArgumentException($"Asset with ID {dto.AssetId} does not exist.");

            var metric = _mapper.Map<MetricData>(dto);
            metric.Timestamp = DateTime.UtcNow;

            _dbContext.MetricData.Add(metric);
            await _dbContext.SaveChangesAsync();

            return _mapper.Map<MetricDataDto>(metric);
        }

        public async Task<MetricDataDto> GetByIdAsync(int id)
        {
            var metric = await _dbContext
                .MetricData.Include(m => m.Asset)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (metric == null)
                return null;

            return _mapper.Map<MetricDataDto>(metric);
        }

        // Get all metrics for an asset
        public async Task<IEnumerable<MetricDataDto>> GetAllAssetMetricsAsync(int assetId)
        {
            // Validate AssetId
            if (!await AssetExistsAsync(assetId))
                throw new ArgumentException($"Asset with ID {assetId} does not exist.");

            var metrics = await _dbContext
                .MetricData.Include(m => m.Asset)
                .Where(m => m.AssetId == assetId)
                .OrderBy(m => m.Timestamp)
                .ToListAsync();

            return _mapper.Map<IEnumerable<MetricDataDto>>(metrics);
        }

        // Get metrics for asset within date range
        public async Task<IEnumerable<MetricDataDto>> GetMetricsByDateRangeAsync(
            int assetId,
            DateTime from,
            DateTime to
        )
        {
            // Validate AssetId
            if (!await AssetExistsAsync(assetId))
                throw new ArgumentException($"Asset with ID {assetId} does not exist.");

            var metrics = await _dbContext
                .MetricData.Include(m => m.Asset)
                .Where(m => m.AssetId == assetId && m.Timestamp >= from && m.Timestamp <= to)
                .OrderBy(m => m.Timestamp)
                .ToListAsync();

            return _mapper.Map<IEnumerable<MetricDataDto>>(metrics);
        }

        public async Task<bool> AssetExistsAsync(int assetId)
        {
            return await _dbContext.Assets.AnyAsync(a => a.Id == assetId);
        }
    }
}

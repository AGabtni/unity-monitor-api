using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Unity.Monitoring.Data;
using Unity.Monitoring.DTO;
using Unity.Monitoring.Models;

namespace Unity.Monitoring.Services
{
    public class AssetService : IAssetService
    {
        private readonly AppDbContext _dbContext;
        private readonly IMapper _mapper;

        public AssetService(AppDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<IEnumerable<AssetDto>> GetAllAsync()
        {
            var assets = await _dbContext.Assets.Include(a => a.Metrics).ToListAsync();
            return _mapper.Map<IEnumerable<AssetDto>>(assets);
        }

        public async Task<AssetDto> GetByIdAsync(int id)
        {
            var asset = await _dbContext
                .Assets.Include(a => a.Metrics)
                .FirstOrDefaultAsync(a => a.Id == id);

            return _mapper.Map<AssetDto>(asset);
        }

        public async Task<IEnumerable<AssetDto>> GetAllByType(EnergyType type)
        {
            var assets = await _dbContext
                .Assets.Where(a => a.Type == type)
                .Include(a => a.Metrics)
                .ToListAsync();

            return _mapper.Map<IEnumerable<AssetDto>>(assets);
        }

        public async Task<AssetDto> CreateAsync(AssetAddDto assetDto)
        {
            var asset = _mapper.Map<Asset>(assetDto);

            _dbContext.Assets.Add(asset);
            await _dbContext.SaveChangesAsync();

            return _mapper.Map<AssetDto>(asset);
        }

        public async Task<AssetDto> UpdateAsync(int id, AssetPutDto assetDto)
        {
            var asset = await _dbContext
                .Assets.Include(a => a.Metrics)
                .FirstOrDefaultAsync(a => a.Id == id);

            if (asset == null)
                return null;

            _mapper.Map(assetDto, asset);

            await _dbContext.SaveChangesAsync();
            return _mapper.Map<AssetDto>(asset);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var existing = await _dbContext.Assets.FindAsync(id);
            if (existing == null)
                return false;

            _dbContext.Assets.Remove(existing);
            await _dbContext.SaveChangesAsync();
            return true;
        }
    }
}

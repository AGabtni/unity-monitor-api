using Unity.Monitoring.DTO;
using Unity.Monitoring.Models;

namespace Unity.Monitoring.Services
{
    public interface IAssetService
    {
        Task<IEnumerable<AssetDto>> GetAllAsync();
        Task<AssetDto> GetByIdAsync(int id);
        Task<IEnumerable<AssetDto>> GetAllByType(EnergyType type);
        Task<AssetDto> CreateAsync(AssetAddDto asset);
        Task<IEnumerable<AssetDto>> CreateBatchAsync(IEnumerable<AssetAddDto> assets);
        Task<AssetDto> UpdateAsync(int id, AssetPutDto asset);
        Task<bool> DeleteAsync(int id);
    }
}

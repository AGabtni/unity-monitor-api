using Microsoft.AspNetCore.Mvc;
using Unity.Monitoring.DTO;
using Unity.Monitoring.Models;
using Unity.Monitoring.Services;

[ApiController]
[Route("api/[controller]")]
public class AssetController : ControllerBase
{
    private readonly IAssetService _assetService;

    public AssetController(IAssetService assetSerice)
    {
        _assetService = assetSerice;
    }

    [HttpPost]
    public async Task<ActionResult<AssetDto>> CreateAsset([FromBody] AssetAddDto asset)
    {
        var created = await _assetService.CreateAsync(asset);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<AssetDto>> UpdateAsset(int id, [FromBody] AssetPutDto asset)
    {
        var updated = await _assetService.UpdateAsync(id, asset);
        if (updated == null)
            return NotFound();
        return updated;
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await _assetService.DeleteAsync(id);
        if (!deleted)
            return NotFound();
        return NoContent();
    }

    [HttpGet]
    public async Task<IEnumerable<AssetDto>> GetAll()
    {
        return await _assetService.GetAllAsync();
    }

    [HttpGet("type/{type}")]
    public async Task<ActionResult<IEnumerable<Asset>>> GetAllByType(EnergyType type)
    {
        var assets = await _assetService.GetAllByType(type);
        if (assets == null || !assets.Any())
            return NotFound();
        return Ok(assets);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<AssetDto>> GetById(int id)
    {
        var asset = await _assetService.GetByIdAsync(id);
        if (asset == null)
            return NotFound();
        return asset;
    }
}

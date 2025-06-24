using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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

    [Authorize(Policy = "AdminRights")]
    [HttpPost()]
    public async Task<ActionResult<AssetDto>> CreateAsset([FromBody] AssetAddDto asset)
    {
        if (asset == null)
            return BadRequest("Please specify an asset to add");
        var created = await _assetService.CreateAsync(asset);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    [Authorize(Policy = "AdminRights")]
    [HttpPost("batch")]
    public async Task<ActionResult<IEnumerable<AssetDto>>> CreateAssets(
        [FromBody] List<AssetAddDto> assets
    )
    {
        if (assets == null || assets.Count == 0)
            return BadRequest("At least one asset is required");

        try
        {
            var createdAssets = await _assetService.CreateBatchAsync(assets);
            return CreatedAtAction(nameof(GetAll), null, createdAssets);
        }
        catch (DbUpdateException ex)
        {
            // Handle database errors
            return StatusCode(500, "Error saving assets: " + ex.Message);
        }
    }

    [Authorize(Policy = "AdminRights")]
    [HttpPut("{id}")]
    public async Task<ActionResult<AssetDto>> UpdateAsset(int id, [FromBody] AssetPutDto asset)
    {
        var updated = await _assetService.UpdateAsync(id, asset);
        if (updated == null)
            return NotFound();
        return updated;
    }

    [Authorize(Policy = "AdminRights")]
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

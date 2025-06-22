using Microsoft.AspNetCore.Mvc;
using Unity.Monitoring.DTO;
using Unity.Monitoring.Services;

[ApiController]
[Route("api/assets/{assetId}/metrics")]
public class MetricDataController : ControllerBase
{
    private readonly IMetricDataService _metricDataService;

    public MetricDataController(IMetricDataService metricDataService)
    {
        _metricDataService = metricDataService;
    }

    [HttpPost]
    public async Task<ActionResult<MetricDataDto>> CreateMetric([FromBody] MetricDataAddDto dto)
    {
        try
        {
            var result = await _metricDataService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetAllAssetMetrics), new { dto.AssetId }, result);
        }
        catch (ArgumentException ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<MetricDataDto>> GetById(int id)
    {
        var metric = await _metricDataService.GetByIdAsync(id);
        if (metric == null)
            return NotFound($"Metric with ID {id}");

        return Ok(metric);
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<MetricDataDto>>> GetAllAssetMetrics(int assetId)
    {
        try
        {
            var result = await _metricDataService.GetAllAssetMetricsAsync(assetId);
            return Ok(result);
        }
        catch (ArgumentException ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpGet("range")]
    public async Task<ActionResult<IEnumerable<MetricDataDto>>> GetByDateRange(
        int assetId,
        [FromQuery] DateTime from,
        [FromQuery] DateTime to
    )
    {
        try
        {
            if (from > to)
                return BadRequest("The 'from' date must be earlier than 'to'.");

            var result = await _metricDataService.GetMetricsByDateRangeAsync(assetId, from, to);
            return Ok(result);
        }
        catch (ArgumentException ex)
        {
            return NotFound(ex.Message);
        }
    }
}

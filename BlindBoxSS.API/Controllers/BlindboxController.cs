using BlindBoxSS.API.Attributes;
using Microsoft.AspNetCore.Mvc;
using Models;
using Services.Cache;
using Services.Product;

[Route("api/blindboxes")]
[ApiController]
public class BlindBoxController : ControllerBase
{
    private readonly IBlindBoxService _service;
    private readonly IResponseCacheService _responseCacheService;

    public BlindBoxController(IBlindBoxService service, IResponseCacheService responseCacheService)
    {
        _service = service;
        _responseCacheService = responseCacheService;
    }

    // Lấy danh sách tất cả blindboxes 
    [HttpGet]
    public async Task<ActionResult> GetAll()
    {
        var blindBoxes = await _service.GetAllAsync();
        return Ok(blindBoxes);
    }


    /// <summary>
    ///  Lấy danh sách blindboxes có phân trang
    /// </summary>
    /// <param name="pageNumber">Số Trang</param>
    /// <param name="pageSize">Số lượng BlindBox trên mỗi trang</param>
    /// <returns>Danh sách BlindBox </returns>
    [HttpGet("paged")]
    [CacheAttribute(1000)]
    public async Task<IActionResult> GetPaged(string? searchByCategory, string? searchByName, decimal? minPrice, decimal? maxPrice, int pageNumber, int pageSize)
    {
        var result = await _service.GetAllFilter(searchByCategory, searchByName, minPrice, maxPrice, pageNumber, pageSize);
        return Ok(result);
    }

    // Lấy một blindbox theo ID
    [HttpGet("{id}")]
    public async Task<ActionResult<BlindBox>> GetById(Guid id)
    {
        var blindBox = await _service.GetByIdAsync(id);
        if (blindBox == null)
        {
            return NotFound();
        }
        return Ok(blindBox);
    }

    // Tạo một blindbox mới
    [HttpPost]
    public async Task<ActionResult<BlindBox>> Create(BlindBox blindBox)
    {
        var createdBlindBox = await _service.AddAsync(blindBox);
        return CreatedAtAction(nameof(GetById), new { id = createdBlindBox.BlindBoxId }, createdBlindBox);
    }

    // Cập nhật blindbox theo ID
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, BlindBox blindBox)
    {
        if (id != blindBox.BlindBoxId)
        {
            return BadRequest();
        }

        await _service.UpdateAsync(blindBox);
        return NoContent();
    }

    // Xóa blindbox theo ID
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _service.DeleteAsync(id);
        return NoContent();
    }
}

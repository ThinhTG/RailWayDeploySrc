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

    /// <summary>
    /// Get All BlindBoxes Search (Mobile)
    /// </summary>
    /// <param name="searchByCategory">Category NAME</param>
    /// <param name="searchByName">BlindBox Name</param>
    /// <param name="minPrice">giá tiền tối thiểu</param>
    /// <param name="maxPrice">giá tiền tối đa</param>
    /// <returns></returns>
    [HttpGet]
    public async Task<ActionResult> GetAll(string? searchByCategory, string? searchByName, decimal? minPrice, decimal? maxPrice)
    {
        var blindBoxes = await _service.GetAllAsync(searchByCategory, searchByName, minPrice, maxPrice);
        return Ok(blindBoxes);
    }


    /// <summary>
    /// lấy All BlindBox Search (Web)
    /// </summary>
    /// <param name="searchByCategory">Category Name</param>
    /// <param name="searchByName">BlindBox Name</param>
    /// <param name="minPrice">Price tối thiểu</param>
    /// <param name="maxPrice">Price tối đa </param>
    /// <param name="pageNumber">số trang</param>
    /// <param name="pageSize">số Blindbox</param>
    /// <returns></returns>
    [HttpGet("paged")]
    public async Task<IActionResult> GetPaged(string? searchByCategory, string? searchByName, decimal? minPrice, decimal? maxPrice, int pageNumber=1, int pageSize = 6)
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

    // lấy list blindbox theo typeSell
    [HttpGet("typeSell/{typeSell}")]
    public async Task<ActionResult<List<BlindBox>>> GetBlindboxeByTypeSell(string typeSell)
    {
        var blindBoxes = await _service.GetBlindboxeByTypeSell(typeSell);
        return Ok(blindBoxes);
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

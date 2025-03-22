using BlindBoxSS.API.Attributes;
using Microsoft.AspNetCore.Mvc;
using Models;
using Services.Cache;
using Services.DTO;
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
    public async Task<ActionResult> GetAll(string? searchByCategory, string? searchByName, decimal? minPrice, decimal? maxPrice, string? size)
    {
        var blindBoxes = await _service.GetAllAsync(searchByCategory, searchByName, minPrice, maxPrice,size);
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
    public async Task<IActionResult> GetPaged(string? searchByCategory,string? typeSell,string? size, string? searchByName, decimal? minPrice, decimal? maxPrice, int pageNumber=1, int pageSize = 6)
    {
        var result = await _service.GetAllFilter(searchByCategory, typeSell,size, searchByName, minPrice, maxPrice, pageNumber, pageSize);
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
    public async Task<ActionResult<BlindBox>> Create([FromBody] AddBlindBoxDTO addBlindBoxDTO)
    {
        if (addBlindBoxDTO == null)
        {
            return BadRequest("Blindbox data is required.");
        }

        try
        {
            var createdBlindBox= await _service.AddAsync(addBlindBoxDTO);
            return CreatedAtAction(nameof(GetById), new { id = createdBlindBox.BlindBoxId}, createdBlindBox);
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(ex.Message); // 409 Conflict for duplicate VoucherId
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message); // For validation errors
        }
    }

    // Cập nhật blindbox theo ID
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, UpdateBlindBoxDTO updateBlindBoxDTO)
    {
        if (updateBlindBoxDTO == null)
        {
            return BadRequest("Blindbox update data is required.");
        }

        try
        {
            var updatedBlindbox = await _service.UpdateAsync(id, updateBlindBoxDTO);
            if (updatedBlindbox == null)
            {
                return NotFound($"Blindbox with ID {id} not found.");
            }

            return NoContent(); // 204 No Content, no body
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Unexpected error while updating voucher {id}: {ex.Message}");
            return StatusCode(500, "An unexpected error occurred while updating the voucher.");
        }
    }

    // Xóa blindbox theo ID
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _service.DeleteAsync(id);
        return NoContent();
    }

    /// <summary>
    /// tao blindbox V2
    /// </summary>
    /// <param name="addBlindBoxDTOV2"></param>
    /// <returns></returns>
    [HttpPost("v2blindbox")]
    public async Task<ActionResult<BlindBox>> Create([FromBody] AddBlindBoxDTOV2 addBlindBoxDTOV2)
    {
        if (addBlindBoxDTOV2 == null)
        {
            return BadRequest("Blindbox data is required.");
        }

        try
        {
            var createdBlindBoxV2 = await _service.AddAsyncV2(addBlindBoxDTOV2);
            return CreatedAtAction(nameof(GetById), new { id = createdBlindBoxV2.BlindBoxId }, createdBlindBoxV2);
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(ex.Message); // 409 Conflict for duplicate VoucherId
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message); // For validation errors
        }
    }

    /// <summary>
    /// update blindbox V2
    /// </summary>
    /// <param name="id"></param>
    /// <param name="updateBlindBoxDTOV2"></param>
    /// <returns></returns>
    [HttpPut("v2/{id}")]
    public async Task<IActionResult> Update(Guid id, UpdateBlindBoxDTOV2 updateBlindBoxDTOV2)
    {
        if (updateBlindBoxDTOV2 == null)
        {
            return BadRequest("Blindbox update data is required.");
        }

        try
        {
            var updatedBlindboxV2 = await _service.UpdateAsyncV2(id, updateBlindBoxDTOV2);
            if (updatedBlindboxV2 == null)
            {
                return NotFound($"Blindbox with ID {id} not found.");
            }

            return NoContent(); // 204 No Content, no body
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Unexpected error while updating voucher {id}: {ex.Message}");
            return StatusCode(500, "An unexpected error occurred while updating the voucher.");
        }
    }

}

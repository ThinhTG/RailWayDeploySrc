using BlindBoxSS.API.Attributes;
using Microsoft.AspNetCore.Mvc;
using Models;
using Services.Cache;
using Services.DTO;
using Services.Product;

[Route("api/packages")]
[ApiController]
public class PackageController : ControllerBase
{
    private readonly IPackageService _packageService;
    private readonly IResponseCacheService _responseCacheService;

    public PackageController(IPackageService packageService, IResponseCacheService responseCacheService )
    {
        _packageService = packageService;
        _responseCacheService = responseCacheService;
    }

    // Lấy gói theo ID
    [HttpGet("{id}")]
    [Cache(100000)]
    public async Task<IActionResult> GetById(Guid id)
    {
        var package = await _packageService.GetPackageByIdAsync(id);
        if (package == null) return NotFound();
        return Ok(package);
    }

    // lấy list package theo typeSell
    [HttpGet("typeSell/{typeSell}")]
    [Cache(100000)]
    public async Task<ActionResult<List<Package>>> GetPackageByTypeSell(string typeSell)
    {
        var packages = await _packageService.GetPackageByTypeSell(typeSell);
        return Ok(packages);
    }

    // Tạo gói mới
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreatePackageRequest package)
    {
        var package1 = new Package
        {
            CategoryId = package.CategoryId,
            PackageName = package.PackageName,
            TypeSell = package.TypeSell,
            PackagePrice = package.PackagePrice,
            Description = package.Description,
            Stock = package.Stock,
            Amount = package.Amount,
            PackageStatus = package.PackageStatus
        };
        var createdPackage = await _packageService.AddPackageAsync(package1);
        await _responseCacheService.RemoveCacheResponseAsync("/api/packages");
        await _responseCacheService.RemoveCacheResponseAsync("/api/blindboxes");
        return CreatedAtAction(nameof(GetById), new { id = createdPackage.PackageId }, createdPackage);
    }

    // Cập nhật gói
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdatePackageRequest updatePackageRequest)
    {
        try
        {
            await _packageService.UpdatePackageAsync(id, updatePackageRequest);
            await _responseCacheService.RemoveCacheResponseAsync("/api/packages");
            return NoContent(); // 204 No Content indicates successful update
        }
        catch (KeyNotFoundException)
        {
            return NotFound(); // 404 Not Found if category doesn't exist
        }
    }

    // Xóa gói
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var result = await _packageService.DeletePackageAsync(id);
        await _responseCacheService.RemoveCacheResponseAsync("/api/packages");
        if (!result) return NotFound();

        return NoContent();
    }

    /// <summary>
    /// Get All Packages Search (Mobile)
    /// </summary>
    /// <param name="searchByCategory">Category NAME</param>
    /// <param name="searchByName">Package Name</param>
    /// <param name="minPrice">giá tiền tối thiểu</param>
    /// <param name="maxPrice">giá tiền tối đa</param>
    /// <returns></returns>
    [HttpGet]
    [Cache(100000)]
    public async Task<ActionResult> GetAll(string? searchByCategory, string? searchByName, decimal? minPrice, decimal? maxPrice)
    {
        var packages = await _packageService.GetAllAsync(searchByCategory, searchByName, minPrice, maxPrice);
        return Ok(packages);
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
    [Cache(100000)]
    public async Task<IActionResult> GetPaged(string? typeSell,string? searchByCategory, string? searchByName, decimal? minPrice, decimal? maxPrice, int pageNumber = 1, int pageSize =6)
    {
        var result = await _packageService.GetAllFilter(typeSell,searchByCategory, searchByName, minPrice, maxPrice, pageNumber, pageSize);
        return Ok(result);
    }


    /// <summary>
    /// Update packageV2
    /// </summary>
    /// <param name="id"></param>
    /// <param name="updatePackageDTO"></param>
    /// <returns></returns>
    [HttpPut("v2/{id}")]
    public async Task<IActionResult> UpdateV2(Guid id, [FromBody] UpdatePackageDTO updatePackageDTO)
    {
        try
        {
            await _packageService.UpdatePackageAsyncV2(id, updatePackageDTO);
            await _responseCacheService.RemoveCacheResponseAsync("/api/packages");
            return NoContent(); // 204 No Content indicates successful update
        }
        catch (KeyNotFoundException)
        {
            return NotFound(); // 404 Not Found if category doesn't exist
        }
    }


    
}

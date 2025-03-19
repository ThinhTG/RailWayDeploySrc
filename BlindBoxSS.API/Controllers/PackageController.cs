using BlindBoxSS.API.Attributes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models;
using Services.DTO;
using Services.Product;

[Route("api/packages")]
[ApiController]
public class PackageController : ControllerBase
{
    private readonly IPackageService _packageService;

    public PackageController(IPackageService packageService)
    {
        _packageService = packageService;
    }

    // Lấy tất cả gói (Không cần "getAll")
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var packages = await _packageService.GetAllPackagesAsync();
        return Ok(packages);
    }
    /// <summary>
    ///  Lấy danh sách Packages có phân trang
    /// </summary>
    /// <param name="pageNumber">Số Trang</param>
    /// <param name="pageSize">Số lượng Package trên mỗi trang</param>
    /// <returns>Danh sách Packge </returns>
    [HttpGet("paged")]
    public async Task<IActionResult> GetPaged(int pageNumber = 1, int pageSize = 10)
    {
        var result = await _packageService.GetAll(pageNumber, pageSize);
        return Ok(result);
    }

    // Lấy gói theo ID
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var package = await _packageService.GetPackageByIdAsync(id);
        if (package == null) return NotFound();
        return Ok(package);
    }

    // Tạo gói mới
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreatePackageRequest package)
    {
        var package1 = new Package
        {
            CategoryId = package.CategoryId,
            PackageName = package.PackageName,
            PackagePrice = package.PackagePrice,
            Description = package.Description,
            Stock = package.Stock,
            Amount = package.Amount,
            PackageStatus = package.PackageStatus
        };
        var createdPackage = await _packageService.AddPackageAsync(package1);
        return CreatedAtAction(nameof(GetById), new { id = createdPackage.PackageId }, createdPackage);
    }

    // Cập nhật gói
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] Package package)
    {
        if (id != package.PackageId) return BadRequest();

        var updatedPackage = await _packageService.UpdatePackageAsync(package);
        if (updatedPackage == null) return NotFound();

        return Ok(updatedPackage);
    }

    // Xóa gói
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var result = await _packageService.DeletePackageAsync(id);
        if (!result) return NotFound();

        return NoContent();
    }
}

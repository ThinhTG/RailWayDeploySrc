using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models;
using Services.Product;

[Route("api/packages")]
[ApiController]
[Authorize]
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
    public async Task<IActionResult> Create([FromBody] Package package)
    {
        var createdPackage = await _packageService.AddPackageAsync(package);
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

using Microsoft.AspNetCore.Mvc;
using Models;
using Services.Product;

namespace BlindBoxSS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PackageController : ControllerBase
    {
        private readonly IPackageService _packageService;

        public PackageController(IPackageService packageService)
        {
            _packageService = packageService;
        }

        [HttpGet("getAll")]
        public async Task<IActionResult> GetAllPackages()
        {
            var packages = await _packageService.GetAllPackagesAsync();
            return Ok(packages);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetPackageById(int id)
        {
            var package = await _packageService.GetPackageByIdAsync(id);
            if (package == null)
            {
                return NotFound();
            }
            return Ok(package);
        }

        [HttpPost]
        public async Task<IActionResult> AddPackage([FromBody] Package package)
        {
            var createdPackage = await _packageService.AddPackageAsync(package);
            return CreatedAtAction(nameof(GetPackageById), new { id = createdPackage.PackageId }, createdPackage);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePackage(int id, [FromBody] Package package)
        {
            if (id != package.PackageId)
            {
                return BadRequest();
            }

            var updatedPackage = await _packageService.UpdatePackageAsync(package);
            if (updatedPackage == null)
            {
                return NotFound();
            }

            return Ok(updatedPackage);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePackage(int id)
        {
            var result = await _packageService.DeletePackageAsync(id);
            if (!result)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}

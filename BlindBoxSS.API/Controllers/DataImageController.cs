using Microsoft.AspNetCore.Mvc;
using Services.DTO;
using Services.Product;

namespace BlindBoxSS.API.Controllers
{
    [Route("api/DataImages")]
    [ApiController]
    public class DataImageController : Controller
    {
        private readonly IBlindBoxImageService _blindBoxImageService;
        private readonly IPackageImageService _packageImageService;

        public DataImageController(IBlindBoxImageService blindBoxImageService, IPackageImageService packageImageService)
        {
            _blindBoxImageService = blindBoxImageService;
            _packageImageService = packageImageService;
        }

        [HttpPost("Blindbox-Images")]
        public async Task<IActionResult> AddBlindBoxImage([FromBody] BBImageDTO bbimageDTO)
        {
            if (bbimageDTO == null)
            {
                return BadRequest();
            }

            try
            {
                await _blindBoxImageService.AddBlindBoxImage(bbimageDTO);
                return Ok(new { Message = "Add blindboxImage successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Error = ex.Message });
            }
        }

        [HttpGet("Blindbox-Images")]
        public async Task<IActionResult> GetBlindboxImage(Guid blindboximageId)
        {
            try
            {
                var image = await _blindBoxImageService.GetBlindBoxImages(blindboximageId);
                if (image != null)
                {
                    return Ok(image);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new { Error = ex.Message });
            }

            return BadRequest();
        }

        [HttpPut("Blindbox-Images")]
        public async Task<IActionResult> UpdateBlindboxImage(Guid blindboximageId, string imageUrl)
        {
            try
            {
                bool result = await _blindBoxImageService.UpdateBlindBoxImage(blindboximageId, imageUrl);
                if (result)
                {
                    return Ok(new { Message = "Update blindbox image successfully" });
                }
                else
                {
                    return BadRequest(new { Message = "Fail to update blindbox image" });
                }
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred.", error = ex.Message });
            }
        }

        [HttpPost("Blindbox-Images/{blindbobImageID}")]
        public async Task<IActionResult> DeleteBlindBoxImage(Guid blindbobImageID)
        {
            try
            {
                bool isDeleted = await _blindBoxImageService.DeleteBlindBoxImage(blindbobImageID);
                if (isDeleted)
                {
                    return Ok(new { Message = "Delete blindbox image successfully" });
                }
                else
                {
                    return NotFound(new { Message = "BlindboxImage not found" });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred.", error = ex.Message });
            }
        }

        [HttpPost("Package-Images")]
        public async Task<IActionResult> AddPackageImage([FromBody] PackageImageDTO packageImageDTO)
        {
            if (packageImageDTO == null)
            {
                return BadRequest();
            }

            try
            {
                await _packageImageService.AddPackageImage(packageImageDTO);
                return Ok(new { Message = "Add packageImage successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Error = ex.Message });
            }
        }

        [HttpGet("Package-Images")]
        public async Task<IActionResult> GetPackageImage(Guid packageimageId)
        {
            try
            {
                var image = await _packageImageService.GetPackageImages(packageimageId);
                if (image != null)
                {
                    return Ok(image);
                }
            }
            catch (Exception ex)
            {
                return NotFound();
            }

            return NotFound();
        }

        [HttpPut("Pacakge-Images")]
        public async Task<IActionResult> UpdatePackageImage(Guid packageimageId, string imageUrl)
        {
            try
            {
                bool result = await _packageImageService.UpdatePackageImage(packageimageId, imageUrl);
                if (result)
                {
                    return Ok(new { Message = "Update package image successfully" });
                }
                else
                {
                    return BadRequest(new { Message = "Fail to update package image" });
                }
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred.", error = ex.Message });
            }
        }

        [HttpPost("Package-Images/{packageImageID}")]
        public async Task<IActionResult> DeletePackageImage(Guid packageImageID)
        {
            try
            {
                bool isDeleted = await _packageImageService.DeletePackageImage(packageImageID);
                if (isDeleted)
                {
                    return Ok(new { Message = "Delete package image successfully" });
                }
                else
                {
                    return NotFound(new { Message = "PackageImage not found" });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred.", error = ex.Message });
            }
        }
    }
}

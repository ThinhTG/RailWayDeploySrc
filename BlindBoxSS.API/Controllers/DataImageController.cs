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
        public async Task<IActionResult> AddBlindBoxImages([FromBody] List<BBImageDTO> bbimageDTOs)
        {
            if (bbimageDTOs == null || !bbimageDTOs.Any())
            {
                return BadRequest();
            }

            try
            {
                foreach (var bbimageDTO in bbimageDTOs)
                {
                    await _blindBoxImageService.AddBlindBoxImages(bbimageDTO);
                }
                return Ok(new { Message = "Add blindboxImages successfully" });
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

        /// <summary>
        /// get list blindbox image by blindboxId
        /// </summary>
        [HttpGet("Blindbox-Images/Blindbox/{blindboxId}")]
        public async Task<IActionResult> GetBlindBoxImageByBlindBox(Guid blindboxId)
        {
            try
            {
                var image = await _blindBoxImageService.GetBlindBoxImageByBlindBox(blindboxId);
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
        public async Task<IActionResult> AddPackageImages([FromBody] List<PackageImageDTO> packageImageDTOs)
        {
            if (packageImageDTOs == null || !packageImageDTOs.Any())
            {
                return BadRequest();
            }

            try
            {
                foreach (var packageImageDTO in packageImageDTOs)
                {
                    await _packageImageService.AddPackageImages(packageImageDTO);
                }
                return Ok(new { Message = "Add packageImages successfully" });
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

        /// <summary>
        /// get list package image by packageId
        /// </summary>
        [HttpGet("Package-Images/Package/{packageId}")]
        public async Task<IActionResult> GetPackageImageByPackage(Guid packageId)
        {
            try
            {
                var image = await _packageImageService.GetPackageImagesByPackageId(packageId);
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

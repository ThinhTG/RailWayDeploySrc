using BlindBoxSS.API.Attributes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models;
using Services.Cache;
using Services.Product;

namespace BlindBoxSS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class BlindboxController : ControllerBase
    {
        private readonly IBlindBoxService _service;
        private readonly IResponseCacheService _responseCacheService;

        public BlindboxController(IBlindBoxService service, IResponseCacheService responseCacheService)
        {
            _service = service;
            _responseCacheService = responseCacheService;
        }

        [HttpGet("getAll")]
        public async Task<ActionResult> GetAll()
        {
            var blindBoxes = await _service.GetAllAsync();
            return Ok(blindBoxes);
        }

        [HttpGet("GetAll-paged")]
        [CacheAttribute(1000)]
        public async Task<IActionResult> GetAllBlindBoxes(int pageNumber = 1, int pageSize = 10)
        {
            var result = await _service.GetAll(pageNumber, pageSize);
            return Ok(result);
        }

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

        [HttpPost]
        public async Task<ActionResult<BlindBox>> Create(BlindBox blindBox)
        {
            var createdBlindBox = await _service.AddAsync(blindBox);
            return CreatedAtAction(nameof(GetById), new { id = createdBlindBox.BlindBoxId }, createdBlindBox);
        }

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

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _service.DeleteAsync(id);
            return NoContent();
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using Models;
using Services.Product;

namespace BlindBoxSS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlindboxController : ControllerBase
    {
        private readonly IBlindBoxService _service;

        public BlindboxController(IBlindBoxService service)
        {
            _service = service;
        }

        [HttpGet("getAll")]
        public async Task<ActionResult<IEnumerable<BlindBox>>> GetAll()
        {
            var blindBoxes = await _service.GetAllAsync();
            return Ok(blindBoxes);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<BlindBox>> GetById(int id)
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
        public async Task<IActionResult> Update(int id, BlindBox blindBox)
        {
            if (id != blindBox.BlindBoxId)
            {
                return BadRequest();
            }

            await _service.UpdateAsync(blindBox);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _service.DeleteAsync(id);
            return NoContent();
        }
    }
}

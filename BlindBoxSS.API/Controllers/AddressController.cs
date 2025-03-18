using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models;
using Services.AddressS;
using Services.DTO;
using Services.VocherS;

namespace BlindBoxSS.API.Controllers
{
    [Route("api/Address")]
    [ApiController]
    public class AddressController : ControllerBase
    {
        private readonly IAddressService _addressService;

        public AddressController(IAddressService addressService)
        {
            _addressService = addressService;
        }

        /// <summary>
        /// Lấy toàn bộ Address
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Address>>> GetAll()
        {
            var address = await _addressService.GetAllAsync();
            return Ok(address);
        }
        /// <summary>
        /// Lấy danh sách Address có phân trang
        /// </summary>
        [HttpGet("paged")]
        public async Task<IActionResult> GetPaged(int pageNumber = 1, int pageSize = 10)
        {
            var result = await _addressService.GetAll(pageNumber, pageSize);
            return Ok(result);
        }

        /// <summary>
        /// Lấy danh sách Address theo id
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<Address>> GetById(Guid id)
        {
            var address = await _addressService.GetByIdAsync(id);
            if (address == null) return NotFound();
            return Ok(address);
        }

        /// <summary>
        /// Update address theo id
        /// </summary>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAsync(Guid id, [FromBody] UpdateAddressDTO updateAddressDTO)
        {
            if (updateAddressDTO == null)
            {
                return BadRequest("Voucher update data is required.");
            }

            try
            {
                var updateAddress = await _addressService.UpdateAsync(id, updateAddressDTO); 
                if (updateAddress == null)
                {
                    return NotFound($"Address with ID {id} not found.");
                }

                return NoContent(); // 204 No Content, no body
            }
         
            catch (ArgumentException ex)
            {
                Console.WriteLine($"Validation error while updating voucher {id}: {ex.Message}");
                return BadRequest(ex.Message); // e.g., "UpdatedAt must be after CreatedAt."
            }
           

        }

        /// <summary>
        /// Create address
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<Order>> Create([FromBody] AddAddressDTO addAddressDTO)
        {
            if (addAddressDTO == null)
            {
                return BadRequest("Address data is required.");
            }

            try
            {
                var createdAddress = await _addressService.AddAsync(addAddressDTO);
                return CreatedAtAction(nameof(GetById), new { id = createdAddress.AddressId }, createdAddress);
            }

            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message); // For validation errors
            }
        }

        /// <summary>
        /// Xóa Address
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                var address = await _addressService.GetByIdAsync(id);
                if (address == null)
                {
                    return NotFound($"address with ID {id} not found.");
                }

                await _addressService.DeleteAsync(id);
                return NoContent(); // 204 No Content indicates successful deletion
            }
            catch (KeyNotFoundException ex)
            {
                // Handle case where the address doesn't exist
                Console.WriteLine($"Not found while deleting address {id}: {ex.Message}");
                return NotFound(ex.Message);
            }
            
        }
        /// <summary>
        /// lấy address theo AccountId
        /// </summary>
        [HttpGet("account/{accountId}")]
        public async Task<ActionResult<IEnumerable<Address>>> GetByAccountId(string accountId, int pageNumber = 1, int pageSize = 10)
        {
            var address = await _addressService.GetByAccountId(accountId, pageNumber, pageSize);
            return Ok(address);
        }


    }
}

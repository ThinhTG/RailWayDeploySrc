using BlindBoxSS.API.Attributes;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Models;
using Services.Cache;
using Services.DTO;
using Services.VocherS;

namespace BlindBoxSS.API.Controllers
{
    [Route("api/voucher")]
    [ApiController]
    public class VoucherController : ControllerBase
    {
        private readonly IVocherService _voucherService;
        private readonly IResponseCacheService _responseCacheService;

        public VoucherController(IVocherService voucherService, IResponseCacheService responseCacheService)
        {
            _voucherService = voucherService;
            _responseCacheService = responseCacheService;
        }

        /// <summary>
        /// Lấy danh sách voucher có thể sử dụng
        /// </summary>
        /// <param name="totalPrice">Total Price của Order</param>
        /// <returns>các Voucher còn hạn sử dụng và đủ điều kiện</returns>
        [HttpGet("available-voucher")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Cache(10000)]
        public async Task<Results<Ok<List<VoucherResponse>>, NotFound>> GetAvailableVouchers([FromQuery] decimal totalPrice)
        {
            return await _voucherService.GetAvailableVouchersAsync(totalPrice);
        }

        /// <summary>
        /// Lấy toàn bộ voucher
        /// </summary>
        [HttpGet]
        [Cache(10000)]
        public async Task<ActionResult<IEnumerable<Voucher>>> GetAll()
        {
            var vouchers = await _voucherService.GetAllVouchersAsync();
            return Ok(vouchers);
        }


        /// <summary>
        /// Lấy Tất Cả Voucher Phân Trang
        /// </summary>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        [HttpGet("paged")]
        [Cache(10000)]
        public async Task<IActionResult> GetPaged(int pageNumber = 1, int pageSize = 10)
        {
            var result = await _voucherService.GetAll(pageNumber, pageSize);
            return Ok(result);
        }

        /// <summary>
        /// Lấy danh sách voucher theo id
        /// </summary>
        [HttpGet("{id}")]
        [Cache(10000)]
        public async Task<ActionResult<Voucher>> GetById(Guid id)
        {
            var voucher = await _voucherService.GetVoucherByIdAsync(id);
            if (voucher == null) return NotFound();
            return Ok(voucher);
        }

        /// <summary>
        /// Update voucher theo id
        /// </summary>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateVoucher(Guid id, [FromBody] UpdateVoucherDTO updateVoucherDto)
        {
            if (updateVoucherDto == null)
            {
                return BadRequest("Voucher update data is required.");
            }

            try
            {
                var updatedVoucher = await _voucherService.UpdateVoucherAsync(id, updateVoucherDto);
                await _responseCacheService.RemoveCacheResponseAsync("/api/voucher");
                if (updatedVoucher == null)
                {
                    return NotFound($"Voucher with ID {id} not found.");
                }

                return NoContent(); // 204 No Content, no body
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine($"Invalid operation while updating voucher {id}: {ex.Message}");
                return BadRequest(ex.Message); // e.g., "Cannot update a voucher that is already associated with an order."
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine($"Validation error while updating voucher {id}: {ex.Message}");
                return BadRequest(ex.Message); // e.g., "EndDate must be after StartDate."
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unexpected error while updating voucher {id}: {ex.Message}");
                return StatusCode(500, "An unexpected error occurred while updating the voucher.");
            }
        }

        /// <summary>
        /// Create voucher
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<Voucher>> Create([FromBody] AddVoucherDTO createVocherDto)
        {
            if (createVocherDto == null)
            {
                return BadRequest("Voucher data is required.");
            }

            try
            {
                var createdVoucher = await _voucherService.AddVoucherAsync(createVocherDto);
                await _responseCacheService.RemoveCacheResponseAsync("/api/voucher");
                return CreatedAtAction(nameof(GetById), new { id = createdVoucher.VoucherId }, createdVoucher);
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
        /// Xóa voucher
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                var voucher = await _voucherService.GetVoucherByIdAsync(id);
                if (voucher == null)
                {
                    return NotFound($"Voucher with ID {id} not found.");
                }

                await _voucherService.DeleteVoucherAsync(id);
                await _responseCacheService.RemoveCacheResponseAsync("/api/voucher");
                return NoContent(); // 204 No Content indicates successful deletion
            }
            catch (KeyNotFoundException ex)
            {
                // Handle case where the voucher doesn't exist
                Console.WriteLine($"Not found while deleting voucher {id}: {ex.Message}");
                return NotFound(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                // Handle case where the voucher is associated with an order
                Console.WriteLine($"Invalid operation while deleting voucher {id}: {ex.Message}");
                return BadRequest(ex.Message); // e.g., "Cannot delete a voucher that is already associated with an order."
            }
            catch (Exception ex)
            {
                // Handle unexpected errors
                Console.WriteLine($"Unexpected error while deleting voucher {id}: {ex.Message}");
                return StatusCode(500, "An unexpected error occurred while deleting the voucher.");
            }
        }
    }


}


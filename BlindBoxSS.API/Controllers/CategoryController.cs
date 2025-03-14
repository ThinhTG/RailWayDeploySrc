using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models;
using Services.CategoryS;
using Services.DTO;
using Services.OrderS;

namespace BlindBoxSS.API.Controllers
{
    [Route("api/Category")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoryController (ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }
        /// <summary>
        ///  Lấy toàn bộ category
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Category>>> GetAll()
        {
            var category = await _categoryService.GetAllCategoryAsync();
            return Ok(category);
        }
        /// <summary>
        ///  Lấy danh sách category có phân trang
        /// </summary>
        [HttpGet("paged")]
        public async Task<IActionResult> GetPaged(int pageNumber = 1, int pageSize = 10)
        {
            var result = await _categoryService.GetAll(pageNumber, pageSize);
            return Ok(result);
        }
        /// <summary>
        ///  Lấy danh sách category theo id
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<Category>> GetById(Guid id)
        {
            var category = await _categoryService.GetCategoryByIdAsync(id);
            if (category == null) return NotFound();
            return Ok(category);
        }
        /// <summary>
        ///  update category theo id
        /// </summary>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCategory(Guid id, [FromBody] UpdateCategoryDTO updateDto)
        {
            try
            {
                await _categoryService.UpdateCategoryAsync(id, updateDto);
                return NoContent(); // 204 No Content indicates successful update
            }
            catch (KeyNotFoundException)
            {
                return NotFound(); // 404 Not Found if category doesn't exist
            }
        }
        /// <summary>
        ///  create category
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<Category>> Create([FromBody] AddCategoryDTO createDto)
        {
            if (createDto == null)
            {
                return BadRequest("Category data is required.");
            }
            var createdCategory = await _categoryService.AddPCategoryAsync(createDto);
            return CreatedAtAction(nameof(GetById), new { id = createdCategory.CategoryId }, createdCategory);
        }

        /// <summary>
        ///  Xóa category
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _categoryService.DeleteCategoryAsync(id);
            return NoContent();
        }
    }
}

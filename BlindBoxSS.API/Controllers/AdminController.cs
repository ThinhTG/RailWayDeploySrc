using DAO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BlindBoxSS.API.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : Controller
    {
        private readonly BlindBoxDbContext _context;

        public AdminController(BlindBoxDbContext context)
        {
            _context = context;
        }

        [Authorize(Roles = "admin")]
        [HttpGet("GetAllUsers")]
        public IActionResult GetAllUsers()
        {
            // check if user is admin
            if (!User.IsInRole("admin"))
            {
                return BadRequest("You are not authorized to access this resource");
            }
            var users = _context.Accounts.ToList();
            return Ok(users);
        }
    }
}

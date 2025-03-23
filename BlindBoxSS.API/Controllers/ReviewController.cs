using DAO.Contracts;
using Microsoft.AspNetCore.Mvc;
using Models;
using Services.ReviewService;

namespace BlindBoxSS.API.Controllers
{
    [Route("api/reviews")]
    [ApiController]

    public class ReviewController : Controller
    {
        private readonly IReviewService _reviewService;


        public ReviewController(IReviewService reviewService)
        {
            _reviewService = reviewService;
        }


        /// <summary>
        /// Lấy Review Theo OrderDetailId
        /// </summary>
        /// <param name="orderDetailId"></param>
        /// <returns>Review của Order đó </returns>
        [HttpGet("/OrderDetail/{orderDetailId}")]
        public async Task<IActionResult> GetReviews(Guid orderDetailId)
        {
            var reviews = await _reviewService.GetReviewsByOrderDetailIdAsync(orderDetailId);
            return Ok(reviews);
        }

        /// <summary>
        /// Lấy Review của BlindBox ( By BlindBoxId)
        /// </summary>
        /// <param name="BlindBoxId"></param>
        /// <returns></returns>
        [HttpGet("/Blindbox/{BlindBoxId}")]
        public async Task<IActionResult> GetReviewsByBlindBoxId(Guid BlindBoxId)
        {
            var reviews = await _reviewService.GetReviewsByBlindBoxIdAsync(BlindBoxId);
            return Ok(reviews);
        }

        /// <summary>
        /// Tạo Review mới
        /// </summary>
        /// <param name="review"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> CreateReview([FromBody] ReviewRequest review)
        {

            var NewReview = await _reviewService.CreateReviewAsync(review);
            if (NewReview == null) return BadRequest("Không thể tạo đánh giá!");
            return Ok(NewReview);
        }


        /// <summary>
        /// Admin Xác nhận Review
        /// </summary>
        /// <param name="reviewId"></param>
        /// <returns></returns>
        [HttpPut("{reviewId}/approve")]
        public async Task<IActionResult> ApproveReview(Guid reviewId)
        {
            var success = await _reviewService.ApproveReviewAsync(reviewId);
            if (!success) return NotFound("Review không tồn tại!");
            return Ok("Review đã được duyệt!");
        }

        /// <summary>
        /// Admin Lấy Tất Cả Review Để Xác Nhận
        /// </summary>
        /// <returns></returns>
        [HttpGet("all")]
        public async Task<IActionResult> GetAllReviews()
        {
            var reviews = await _reviewService.GetAllReviewsAsync();
            return Ok(reviews);
        }

        [HttpGet("/Package/{PackageId}")]
        public async Task<IActionResult> GetReviewsByPackageId(Guid PackageId)
        {
            var reviews = await _reviewService.GetAllReviewsByPackageIdAsync(PackageId);
            return Ok(reviews);
        }


    }
}

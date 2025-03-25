using DAO.Contracts;
using Microsoft.AspNetCore.Mvc;
using Models;
using Services.DTO;
using Services.OrderS;
using Services.Product;
using Services.ReviewService;

namespace BlindBoxSS.API.Controllers
{
    [Route("api/reviews")]
    [ApiController]

    public class ReviewController : Controller
    {
        private readonly IReviewService _reviewService;
        private readonly IOrderService _orderService;
        private readonly IOrderDetailService _orderDetailService;
        private readonly IBlindBoxService _blindBoxService;
        private readonly IPackageService _packageService;


        public ReviewController(IReviewService reviewService, IOrderService orderService, IBlindBoxService blindBoxService, IPackageService packageService, IOrderDetailService orderDetailService)
        {
            _reviewService = reviewService;
            _orderService = orderService;
            _blindBoxService = blindBoxService;
            _packageService = packageService;
            _orderDetailService = orderDetailService;
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
            var orderDetail = await _orderDetailService.GetOrderDetailByIdAsync(review.OrderDetailId);
            if (orderDetail == null) return BadRequest("OrderDetail không tồn tại!");

            var blindbox = await _blindBoxService.GetByIdAsync(orderDetail.BlindBoxId);
            if (blindbox == null) return BadRequest("BlindBox không tồn tại!");

            // Reset the price if the BlindBox is marked as secret
            if (blindbox.isSecret)
            {
                var package = await _packageService.GetPackageByIdAsync(orderDetail.PackageId);
                if (package == null) return BadRequest("Package không tồn tại!");

                // Ensure DefaultPrice and PackagePrice are correctly set
                if (package.DefaultPrice > 0) // Prevent overwriting with 0
                {
                    var updatedPackage = new UpdatePackageRequest
                    {
                        CategoryId = package.CategoryId,
                        PackageName = package.PackageName,
                        TypeSell = package.TypeSell,
                        PackagePrice = package.DefaultPrice, // Reset to the default price
                        Description = package.Description,
                        Stock = package.Stock,
                        Amount = package.Amount,
                        PackageStatus = package.PackageStatus,
                        DefaultPrice = package.DefaultPrice // Ensure DefaultPrice is retained
                    };
                    await _packageService.UpdatePackageAsync(package.PackageId, updatedPackage);
                }
            }

            var newReview = new ReviewRequest
            {
                OrderDetailId = review.OrderDetailId,
                AccountId = review.AccountId,
                Rating = review.Rating,
                Comment = review.Comment,
                ImageURL = review.ImageURL,
            };

            var createdReview = await _reviewService.CreateReviewAsync(newReview);
            if (createdReview == null) return BadRequest("Không thể tạo đánh giá!");

            return Ok(createdReview);
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

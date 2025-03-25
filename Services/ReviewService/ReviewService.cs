using AutoMapper;
using DAO.Contracts;
using DAO.Contracts.ReviewResponses;
using Microsoft.AspNetCore.Identity;
using Models;
using Repositories.ReviewRepo;

namespace Services.ReviewService
{
    public class ReviewService :IReviewService
    {
        private readonly IReviewRepository _reviewRepository;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMapper _mapper;

        public ReviewService(IReviewRepository reviewRepository, IMapper mapper, UserManager<ApplicationUser> userManager)
        {
            _reviewRepository = reviewRepository;
            _mapper = mapper;
            _userManager = userManager;
        }

        public async Task<IEnumerable<ReviewResponse>> GetReviewsByOrderDetailIdAsync(Guid orderDetailId)
        {
            var reviews = await _reviewRepository.GetReviewsByOrderDetailIdAsync(orderDetailId);
            var reviewResponses = new List<ReviewResponse>();

            foreach (var review in reviews)
            {
                ApplicationUser account = null;
                if (review.AccountId != null)
                {
                    account = await _userManager.FindByIdAsync(review.AccountId);
                }

                var reviewResponse = new ReviewResponse
                {
                    ReviewId = review.ReviewId,
                    OrderDetailId = review.OrderDetailId,
                    Account = account != null ? new AccountReviewResponse
                    {
                        FirstName = account.FirstName,
                        LastName = account.LastName,
                        AvatarURL = account.AvatarURL
                    } : null,
                    Rating = review.Rating,
                    Comment = review.Comment,
                    CreateAt = review.CreateAt,
                    ReviewStatus = review.ReviewStatus,
                    ImageUrl = review.imageUrl
                };

                reviewResponses.Add(reviewResponse);
            }

            return reviewResponses;
        }

        public async Task<ReviewResponse> GetReviewByIdAsync(Guid reviewId)
        {
            var review = await _reviewRepository.GetReviewByIdAsync(reviewId);
            return _mapper.Map<Review, ReviewResponse>(review);
        }

        public async Task<ReviewResponse> CreateReviewAsync(ReviewRequest review)
        {
            if (review == null || review.OrderDetailId == Guid.Empty)
                throw new ArgumentNullException(nameof(review), "Review or OrderDetailId is null");

            var existingReview = await _reviewRepository.GetReviewsByOrderDetailIdAsync(review.OrderDetailId);

            if (existingReview.Count() >0)
            {
                throw new InvalidOperationException("Review for this OrderDetailId already exists.");
            }

            // Tìm account bằng await thay vì .Result
            var account = await _userManager.FindByIdAsync(review.AccountId);
            if (account == null)
            {
                throw new KeyNotFoundException("Account not found.");
            }

            // Tạo đối tượng Review mới
            var newReview = new Review
            {
                ReviewId = Guid.NewGuid(),
                OrderDetailId = review.OrderDetailId,
                AccountId = review.AccountId,
                Account = account,  // Gán Account để có dữ liệu khi trả về
                Rating = review.Rating,
                Comment = review.Comment,
                imageUrl = review.ImageURL,
                CreateAt = DateTime.UtcNow,
                ReviewStatus = "Pending"
                
            };

            await _reviewRepository.AddReviewAsync(newReview);
            await _reviewRepository.SaveChangesAsync();

            // Map sang ReviewResponse
            var reviewResponse = new ReviewResponse
            {
                ReviewId = newReview.ReviewId,
                OrderDetailId = newReview.OrderDetailId,
                Account = new AccountReviewResponse // Nếu không muốn trả về toàn bộ Account, chỉ lấy dữ liệu cần thiết
                {
                    FirstName = account.FirstName,
                    LastName = account.LastName,
                    AvatarURL = account.AvatarURL
                },
                Rating = newReview.Rating,
                Comment = newReview.Comment,
                CreateAt = newReview.CreateAt,
                ReviewStatus = newReview.ReviewStatus,
                ImageUrl = newReview.imageUrl
            };

            return reviewResponse;
        }


        public async Task<bool> ApproveReviewAsync(Guid reviewId)
        {
            var review = await _reviewRepository.GetReviewByIdAsync(reviewId);
            if (review == null) { throw new KeyNotFoundException("Review Not Found");  } 
            review.ReviewStatus = "Approved";
            await _reviewRepository.UpdateReviewAsync(review);
            await _reviewRepository.SaveChangesAsync();
            return true;
        }


        //public async Task<IEnumerable<ReviewResponse>> GetReviewsByBlindBoxIdAsync(Guid blindBoxId)
        //{
        //    var reviews = await _reviewRepository.GetReviewsByBlindBoxIdAsync(blindBoxId);
        //    return _mapper.Map<IEnumerable<Review>, IEnumerable<ReviewResponse>>(reviews);
        //}

        // ✅ Lấy tất cả Review
        public async Task<IEnumerable<Review>> GetAllReviewsAsync()
        {
            return await _reviewRepository.GetAllReviewsAsync();
        }


        public async Task<IEnumerable<ReviewResponse>> GetAllReviewsByPackageIdAsync(Guid packageId)
        {
            var reviews = await _reviewRepository.GetAllReviewsByPackageIdAsync(packageId);
            var reviewResponses = new List<ReviewResponse>();
            foreach (var review in reviews)
            {
                ApplicationUser account = null;
                if (review.AccountId != null)
                {
                    account = await _userManager.FindByIdAsync(review.AccountId);
                }

                var reviewResponse = new ReviewResponse
                {
                    ReviewId = review.ReviewId,
                    OrderDetailId = review.OrderDetailId,
                    Account = account != null ? new AccountReviewResponse
                    {
                        FirstName = account.FirstName,
                        LastName = account.LastName,
                        AvatarURL = account.AvatarURL
                    } : null,
                    Rating = review.Rating,
                    Comment = review.Comment,
                    CreateAt = review.CreateAt,
                    ReviewStatus = review.ReviewStatus,
                    ImageUrl = review.imageUrl
                };

                reviewResponses.Add(reviewResponse);
            }
            return reviewResponses;
        }


        public async Task<IEnumerable<ReviewResponse>> GetReviewsByBlindBoxIdAsync(Guid blindBoxId)
        {
            var reviews = await _reviewRepository.GetReviewsByBlindBoxId(blindBoxId);
            var reviewResponses = new List<ReviewResponse>();

            foreach (var review in reviews)
            {
                ApplicationUser account = null;
                if (review.AccountId != null)
                {
                    account = await _userManager.FindByIdAsync(review.AccountId);
                }

                var reviewResponse = new ReviewResponse
                {
                    ReviewId = review.ReviewId,
                    OrderDetailId = review.OrderDetailId,
                    Account = account != null ? new AccountReviewResponse
                    {
                        FirstName = account.FirstName,
                        LastName = account.LastName,
                        AvatarURL = account.AvatarURL
                    } : null,
                    Rating = review.Rating,
                    Comment = review.Comment,
                    CreateAt = review.CreateAt,
                    ReviewStatus = review.ReviewStatus,
                    ImageUrl = review.imageUrl
                };

                reviewResponses.Add(reviewResponse);
            }
            return reviewResponses;
        }

    }
}

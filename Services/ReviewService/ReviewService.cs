using AutoMapper;
using DAO.Contracts;
using Microsoft.EntityFrameworkCore;
using Models;
using Repositories.ReviewRepo;

namespace Services.ReviewService
{
    public class ReviewService :IReviewService
    {
        private readonly IReviewRepository _reviewRepository;
        private readonly IMapper _mapper;

        public ReviewService(IReviewRepository reviewRepository, IMapper mapper)
        {
            _reviewRepository = reviewRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ReviewResponse>> GetReviewsByOrderDetailIdAsync(Guid orderDetailId)
        {
            var reviews = await _reviewRepository.GetReviewsByOrderDetailIdAsync(orderDetailId);
            return _mapper.Map<IEnumerable<Review>, IEnumerable<ReviewResponse>>(reviews);
        }

        public async Task<ReviewResponse> GetReviewByIdAsync(Guid reviewId)
        {
            var review = await _reviewRepository.GetReviewByIdAsync(reviewId);
            return _mapper.Map<Review, ReviewResponse>(review);
        }

        public async Task<ReviewResponse> CreateReviewAsync(ReviewRequest review)
        {
            if (review == null || review.OrderDetailId == Guid.Empty)
                throw new ArgumentNullException("Review or OrderDetailId is null");

            var existingReview = await _reviewRepository.GetReviewsByOrderDetailIdAsync(review.OrderDetailId);

            if (existingReview.Count() > 0)
            {
                throw new InvalidOperationException("Review for this OrderDetailId already exists.");
            }

            var newReview = _mapper.Map<ReviewRequest, Review>(review);

            await _reviewRepository.AddReviewAsync(newReview);
            await _reviewRepository.SaveChangesAsync();
            return _mapper.Map<Review, ReviewResponse>(newReview);
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

        // ✅ Lấy Review theo BlindBoxId
        public async Task<IEnumerable<Review>> GetAllReviewsByBlindBoxIdAsync(Guid blindBoxId)
        {
            return await _reviewRepository.GetAllReviewsByBlindBoxIdAsync(blindBoxId);
        }

        // ✅ Lấy Review theo PackageId
        public async Task<IEnumerable<Review>> GetAllReviewsByPackageIdAsync(Guid packageId)
        {
            return await _reviewRepository.GetAllReviewsByPackageIdAsync(packageId);
        }

    }
}

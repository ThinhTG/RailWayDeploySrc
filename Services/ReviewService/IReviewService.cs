using DAO.Contracts;
using Models;

namespace Services.ReviewService
{
    public interface IReviewService
    {
        Task<IEnumerable<ReviewResponse>> GetReviewsByOrderDetailIdAsync(Guid orderDetailId);
        Task<ReviewResponse> GetReviewByIdAsync(Guid reviewId);
        Task<bool> CreateReviewAsync(ReviewRequest review);
        Task<bool> ApproveReviewAsync(Guid reviewId);

        //Task<IEnumerable<ReviewResponse>> GetReviewsByBlindBoxIdAsync(Guid blindBoxId);
        Task<IEnumerable<Review>> GetAllReviewsAsync();
        Task<IEnumerable<Review>> GetAllReviewsByBlindBoxIdAsync(Guid blindBoxId);
        Task<IEnumerable<Review>> GetAllReviewsByPackageIdAsync(Guid packageId);
    }
}

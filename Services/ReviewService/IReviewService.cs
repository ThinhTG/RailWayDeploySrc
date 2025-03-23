using DAO.Contracts;
using Models;

namespace Services.ReviewService
{
    public interface IReviewService
    {
        Task<IEnumerable<ReviewResponse>> GetReviewsByOrderDetailIdAsync(Guid orderDetailId);
        Task<ReviewResponse> GetReviewByIdAsync(Guid reviewId);
        Task<ReviewResponse> CreateReviewAsync(ReviewRequest review);
        Task<bool> ApproveReviewAsync(Guid reviewId);

        //Task<IEnumerable<ReviewResponse>> GetReviewsByBlindBoxIdAsync(Guid blindBoxId);
        Task<IEnumerable<Review>> GetAllReviewsAsync();
        Task<IEnumerable<ReviewResponse>> GetReviewsByBlindBoxIdAsync(Guid blindBoxId);

        Task<IEnumerable<ReviewResponse>> GetAllReviewsByPackageIdAsync(Guid packageId);


    }
}

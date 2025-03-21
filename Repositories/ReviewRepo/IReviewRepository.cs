using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.ReviewRepo
{
    public interface IReviewRepository
    {
        Task<IEnumerable<Review>> GetReviewsByOrderDetailIdAsync(Guid orderDetailId);
        Task<Review> GetReviewByIdAsync(Guid reviewId);
        Task AddReviewAsync(Review review);
        Task UpdateReviewAsync(Review review);
        Task SaveChangesAsync();

        //Task<IEnumerable<Review>> GetReviewsByBlindBoxIdAsync(Guid blindBoxId);
        Task<IEnumerable<Review>> GetAllReviewsAsync();
        Task<IEnumerable<Review>> GetAllReviewsByBlindBoxIdAsync(Guid blindBoxId);
        Task<IEnumerable<Review>> GetAllReviewsByPackageIdAsync(Guid packageId);
    }
}

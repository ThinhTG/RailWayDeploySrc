using DAO;
using Microsoft.EntityFrameworkCore;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.ReviewRepo
{
    public class ReviewRepository : IReviewRepository
    {
        private readonly BlindBoxDbContext _DbContext;

        public ReviewRepository(BlindBoxDbContext dbContext)
        {
            _DbContext = dbContext;
        }
        public async Task<IEnumerable<Review>> GetReviewsByOrderDetailIdAsync(Guid orderDetailId)
        {
            return await _DbContext.Reviews
                .Where(r => r.OrderDetailId.Equals(orderDetailId))
                .ToListAsync();
        }

        public async Task<Review> GetReviewByIdAsync(Guid reviewId)
        {
            return await _DbContext.Reviews.FindAsync(reviewId);
        }

        public async Task AddReviewAsync(Review review)
        {
            await _DbContext.Reviews.AddAsync(review);
        }

        public async Task UpdateReviewAsync(Review review)
        {
            _DbContext.Reviews.Update(review);
        }

        public async Task SaveChangesAsync()
        {
            await _DbContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<Review>> GetReviewsByBlindBoxIdAsync(Guid blindBoxId)
        {
            return await _DbContext.Reviews
                .Include(r => r.OrderDetail)
                .Where(o => o.ProductId.Equals(blindBoxId))
                .ToListAsync();
        }


    }
}

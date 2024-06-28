using ChillDe.FMS.Repositories.Entities;
using ChillDe.FMS.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChillDe.FMS.Repositories.Enums;

namespace ChillDe.FMS.Repositories.Repositories
{
    public class DeliverableProductRepository : GenericRepository<DeliverableProduct>, IDeliverableProductRepository
    {
        private readonly AppDbContext _dbContext;

        public DeliverableProductRepository(AppDbContext dbContext, IClaimsService claimsService) : base(dbContext, claimsService)
        {
            _dbContext = dbContext;
        }

        public async Task<DeliverableProduct> GetByProjectApplyId(Guid projectApplyId)
        {
            // return await _dbContext.DeliverableProduct.SingleOrDefaultAsync(dp => dp.ProjectApplyId == projectApplyId);
            return await _dbContext.DeliverableProduct.FirstOrDefaultAsync(dp => dp.ProjectApplyId == projectApplyId && dp.Status == DeliverableProductStatus.Accepted);
        }
    }
}

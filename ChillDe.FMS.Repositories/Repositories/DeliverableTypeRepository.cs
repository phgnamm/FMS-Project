using ChillDe.FMS.Repositories;
using ChillDe.FMS.Repositories.Entities;
using ChillDe.FMS.Repositories.Interfaces;
using ChillDe.FMS.Repositories.Repositories;
using Microsoft.EntityFrameworkCore;
using Repositories.Interfaces;

namespace Repositories.Repositories
{
    public class DeliverableTypeRepository : GenericRepository<DeliverableType>, IDeliverableTypeRepository
    {
        private readonly AppDbContext _dbContext;

        public DeliverableTypeRepository(AppDbContext dbContext, IClaimsService claimsService) : 
            base(dbContext, claimsService)
        {
            _dbContext = dbContext;
        }

        public async Task<DeliverableType> GetByName(string name)
        {
            return await _dbContext.DeliverableType.FirstOrDefaultAsync(x => x.Name.ToLower() == name.ToLower());
        }
    }
}

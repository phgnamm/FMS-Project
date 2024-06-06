using ChillDe.FMS.Repositories;
using ChillDe.FMS.Repositories.Entities;
using ChillDe.FMS.Repositories.Interfaces;
using ChillDe.FMS.Repositories.Repositories;
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
    }
}

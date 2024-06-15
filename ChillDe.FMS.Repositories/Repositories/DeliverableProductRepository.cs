using ChillDe.FMS.Repositories.Entities;
using ChillDe.FMS.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChillDe.FMS.Repositories.Repositories
{
    public class DeliverableProductRepository : GenericRepository<DeliverableProduct>, IDeliverableProductRepository
    {
        private readonly AppDbContext _dbContext;

        public DeliverableProductRepository(AppDbContext dbContext, IClaimsService claimsService) : base(dbContext, claimsService)
        {
            _dbContext = dbContext;
        }
    }
}

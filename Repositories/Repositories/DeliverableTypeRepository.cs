using Repositories.Entities;
using Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

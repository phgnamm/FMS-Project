using ChillDe.FMS.Repositories.Entities;
using ChillDe.FMS.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChillDe.FMS.Repositories.Repositories
{
    public class FreelancerSkillRepository : GenericRepository<FreelancerSkill>, IFreelancerSkillRepository
    {
        private readonly AppDbContext _dbContext;

        public FreelancerSkillRepository(AppDbContext dbContext, IClaimsService claimsService) : base(dbContext, claimsService)
        {
            _dbContext = dbContext;
        }

        public List<FreelancerSkill> GetFreelancerSKill(Guid id)
        {
            return _dbContext.FreelancerSkill.Where(x => x.FreelancerId == id).ToList();
        } 
    }
}

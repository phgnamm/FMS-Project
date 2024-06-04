using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChillDe.FMS.Repositories.Entities;
using ChillDe.FMS.Repositories.Interfaces;

namespace ChillDe.FMS.Repositories.Repositories
{
    public class SkillRepository : GenericRepository<Skill>, ISkillRepository
    {
        private readonly AppDbContext _dbContext;

        public SkillRepository(AppDbContext dbContext, IClaimsService claimsService) : base(dbContext, claimsService)
        {
            _dbContext = dbContext;
        }
    }
}

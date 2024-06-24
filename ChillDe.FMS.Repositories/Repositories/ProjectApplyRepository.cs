using ChillDe.FMS.Repositories.Entities;
using ChillDe.FMS.Repositories.Enums;
using ChillDe.FMS.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChillDe.FMS.Repositories.Repositories
{
    public class ProjectApplyRepository : GenericRepository<ProjectApply>, IProjectApplyRepository
    {
        private readonly AppDbContext _dbContext;

        public ProjectApplyRepository(AppDbContext dbContext, IClaimsService claimsService) : base(dbContext, claimsService)
        {
            _dbContext = dbContext;
        }

        public async Task<ProjectApply> GetAcceptedProjectApplyByProjectId(Guid projectId)
        {
            return await _dbContext.ProjectApply
                .SingleOrDefaultAsync(pa => pa.ProjectId == projectId && pa.Status == ProjectApplyStatus.Accepted);
        }
    }
}

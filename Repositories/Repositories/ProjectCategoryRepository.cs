using Microsoft.EntityFrameworkCore;
using Repositories.Entities;
using Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Repositories
{
    public class ProjectCategoryRepository : GenericRepository<ProjectCategory>, IProjectCategoryReposioty
    {
        private readonly AppDbContext _dbContext;

        public ProjectCategoryRepository(AppDbContext dbContext, IClaimsService claimsService) : base(dbContext, claimsService)
        {
            _dbContext = dbContext;
        }

        public async Task<ProjectCategory> GetByName(string name)
        {
            var projectCategory = await _dbContext.ProjectCategory.SingleOrDefaultAsync(x => x.Name == name);
            return projectCategory;
        }
    }
}

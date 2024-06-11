using ChillDe.FMS.Repositories;
using ChillDe.FMS.Repositories.Entities;
using ChillDe.FMS.Repositories.Interfaces;
using ChillDe.FMS.Repositories.Repositories;
using Microsoft.EntityFrameworkCore;
using Repositories.Interfaces;
namespace Repositories.Repositories
{
    public class ProjectCategoryRepository : GenericRepository<ProjectCategory>, IProjectCategoryRepository
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

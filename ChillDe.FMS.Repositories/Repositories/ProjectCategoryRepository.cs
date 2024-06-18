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

        public async Task<List<ProjectCategory>> GetByNames(List<string> names)
        {
            return await _dbContext.ProjectCategory.Where(x => names.Contains(x.Name)).ToListAsync();
        }
    }
}

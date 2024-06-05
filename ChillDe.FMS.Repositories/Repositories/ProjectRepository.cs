using ChillDe.FMS.Repositories.Entities;
using ChillDe.FMS.Repositories.Enums;
using ChillDe.FMS.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ChillDe.FMS.Repositories.Repositories;

public class ProjectRepository : GenericRepository<Project>, IProjectRepository
{
    private readonly AppDbContext _dbContext;
    
    public ProjectRepository(AppDbContext dbContext, IClaimsService claimsService) : base(dbContext, claimsService)
    {
        _dbContext = dbContext;
    }

    public async Task<List<Project>> GetProjectByAccount(Guid id, bool isDeleted, ProjectStatus[] projectStatusList)
    {
        var projectList = _dbContext.Project.Where(x => x.AccountId == id);

        if (isDeleted != null)
        {
            projectList = projectList.Where(x => x.IsDeleted == isDeleted);
        }

        if (projectStatusList != null)
        {
            var statusStrings = projectStatusList.Select(ps => ps.ToString()).ToList();
            projectList = projectList.Where(x => statusStrings.Contains(x.Status));
        }

        return projectList.ToList();
    }
}
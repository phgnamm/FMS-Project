using Microsoft.EntityFrameworkCore;
using Repositories.Entities;
using Repositories.Enums;
using Repositories.Interfaces;

namespace Repositories.Repositories;

public class ProjectRepository : GenericRepository<Project>, IProjectRepository
{
    private readonly AppDbContext _dbContext;
    
    public ProjectRepository(AppDbContext dbContext, IClaimsService claimsService) : base(dbContext, claimsService)
    {
        _dbContext = dbContext;
    }

    public async Task<List<Project>> GetProjectByAccount(Guid id, bool isDeleted, ProjectStatus[] projectStatusList)
    {
        var projectList = await _dbContext.Project.Where(x => x.AccountId == id).ToListAsync();

        if (isDeleted != null)
        {
            projectList = projectList.Where(x => x.IsDeleted == isDeleted).ToList();
        }

        if (projectStatusList != null)
        {
            var statusStrings = projectStatusList.Select(ps => ps.ToString()).ToList();
            projectList = projectList.Where(x => statusStrings.Contains(x.Status)).ToList();
        }

        return projectList;
    }
}
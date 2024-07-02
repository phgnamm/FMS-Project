using ChillDe.FMS.Repositories.Entities;
using ChillDe.FMS.Repositories.Enums;
using ChillDe.FMS.Repositories.Interfaces;
using ChillDe.FMS.Repositories.Models.ProjectModels;
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

        if (projectStatusList != null && projectStatusList.Length > 0)
        {
            projectList = projectList.Where(x => x.Status.HasValue && projectStatusList.Contains(x.Status.Value));
        }

        return projectList.ToList();
    }

    public async Task<Project> GetProjectByCode(string code)
    {
        var project = await _dbContext.Project.FirstOrDefaultAsync(p => code == p.Code && p.IsDeleted == false);
        return project;
    }

    public async Task<int> CountProjectByDeliverableType(Guid id)
    {
        var countProjects = await _dbContext.Project.Where(x =>
            x.IsDeleted == false && x.Status != ProjectStatus.Done && x.Status != ProjectStatus.Closed &&
            x.ProjectDeliverables.Any(x => x.DeliverableType.Id == id)).CountAsync();
        return countProjects;
    }
}
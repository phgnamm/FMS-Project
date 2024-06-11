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
        var project = await _dbContext.Project.FirstOrDefaultAsync(p => code == p.Code);
        return project;
    }

    //public async Task<Project> GetProjectById(string code)
    //{
    //    var project = await _dbContext.Project.FirstOrDefaultAsync(p => code == p.Code);
    //    return project;
    //}

    //public async Task<List<ProjectModel>> GetFreelancersByFilter(PaginationParameter paginationParameter,
    // ProjectModel projectFilterModel)
    //{
    //    var projects = new List<Project>();

    //    // Filter
    //    if (projectFilterModel.Status.HasValue)
    //    {
    //        projects = _dbContext.Project.Where(x => x.Status == projectFilterModel.Status.ToString()).ToList();
    //    }

    //    if (projectFilterModel.Code != null)
    //    {
    //        projects = _dbContext.Project.Where(x => x.Code == projectFilterModel.Code).ToList();
    //    }

    //    if (projectFilterModel.Name != null)
    //    {
    //        projects = _dbContext.Project.Where(p => p.Name == projectFilterModel.Name).ToList();
    //    }

    //    if (projectFilterModel.CategoryId != null)
    //    {
    //        projects = _dbContext.Project.Where(p => p.CategoryId == projectFilterModel.CategoryId).ToList();
    //    }

    //    // Search
    //    if (projectFilterModel.Search != null)
    //    {
    //        projects = projects.Where(x =>
    //            x.Code.ToLower().Contains(projectFilterModel.Search.ToLower()) ||
    //            x.Name.ToLower().Contains(projectFilterModel.Search.ToLower())).ToList();
    //    }

    //    switch (projectFilterModel.Sort?.ToLower())
    //    {
    //        case "name":
    //            projects = projectFilterModel.SortDirection.ToLower() == "asc"
    //                ? projects.OrderBy(x => x.Name).ToList()
    //                : projects.OrderByDescending(x => x.Name).ToList();
    //            break;
    //        case "code":
    //            projects = projectFilterModel.SortDirection.ToLower() == "asc"
    //                ? projects.OrderBy(x => x.Code).ToList()
    //                : projects.OrderByDescending(x => x.Code).ToList();
    //            break;
    //        default:
    //            projects = projects.OrderByDescending(x => x.CreationDate).ToList();
    //            break;
    //    }
    //    var projectModelList = projects
    //      .Select(p => new ProjectDetailModel
    //      {
    //          Code = p.Code,
    //          Name = p.Name,
    //          Description = p.Description,
    //          Duration = p.Duration,
    //          Price = p.Price,
    //          Status = p.Status,
    //          AccountId = p.AccountId,
    //          CategoryId = p.CategoryId
    //      }).ToList();

    //    return projectModelList;
    //}
}
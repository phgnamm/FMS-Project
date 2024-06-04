using Repositories.Entities;
using Repositories.Enums;
using Repositories.ViewModels.ProjectModels;
using Repositories.ViewModels.ResponseModels;

namespace Repositories.Interfaces;

public interface IProjectRepository : IGenericRepository<Project>
{
    Task<List<Project>> GetProjectByAccount(Guid id, bool isDeleted, ProjectStatus[] projectStatusList);
    Task<Project> GetProjectByCode(string code);
}
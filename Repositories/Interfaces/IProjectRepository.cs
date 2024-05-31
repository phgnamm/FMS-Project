using Repositories.Entities;
using Repositories.Enums;

namespace Repositories.Interfaces;

public interface IProjectRepository : IGenericRepository<Project>
{
    Task<List<Project>> GetProjectByAccount(Guid id, bool isDeleted, ProjectStatus[] projectStatusList);
}
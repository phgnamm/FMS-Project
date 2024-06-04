using ChillDe.FMS.Repositories.Entities;
using ChillDe.FMS.Repositories.Enums;

namespace ChillDe.FMS.Repositories.Interfaces;

public interface IProjectRepository : IGenericRepository<Project>
{
    Task<List<Project>> GetProjectByAccount(Guid id, bool isDeleted, ProjectStatus[] projectStatusList);
}
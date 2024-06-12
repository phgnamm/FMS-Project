﻿
using ChillDe.FMS.Repositories.Entities;
using ChillDe.FMS.Repositories.Interfaces;

namespace Repositories.Interfaces
{
    public interface IProjectCategoryRepository : IGenericRepository<ProjectCategory>
    {
        Task<List<ProjectCategory>> GetByNames(List<string> names);
    }
}

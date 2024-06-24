using ChillDe.FMS.Repositories;
using ChillDe.FMS.Repositories.Entities;
using ChillDe.FMS.Repositories.Interfaces;
using ChillDe.FMS.Repositories.Models.QueryModels;
using ChillDe.FMS.Repositories.Repositories;
using Microsoft.EntityFrameworkCore;
using Repositories.Interfaces;
using Repositories.ViewModels.ProjectCategoryModels;
using System.Collections.Generic;
using System.Linq.Expressions;

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
        public async Task<QueryResultModel<List<ProjectCategoryFilterResultModel>>> GetProCateByFilter(
            Expression<Func<ProjectCategoryFilterResultModel, bool>> filter = null,
            Func<IQueryable<ProjectCategoryFilterResultModel>, IOrderedQueryable<ProjectCategoryFilterResultModel>> orderBy = null,
            string includeProperties = "",
            int? pageIndex = null, // Optional parameter for pagination (page number)
            int? pageSize = null)
        {
            int totalCount = 0;

            IQueryable<ProjectCategoryFilterResultModel> query = _dbContext.ProjectCategory.Select(s => new ProjectCategoryFilterResultModel()
            {
                Id = s.Id,
                Name = s.Name,
                Description = s.Description,
                IsDeleted = s.IsDeleted,
                CreationDate = s.CreationDate,
            });

            if (filter != null)
            {
                query = query.Where(filter);
            }
            totalCount = await query.CountAsync();
            foreach (var includeProperty in includeProperties.Split
                (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }

            if (orderBy != null)
            {
                query = orderBy(query);
            }

            // Implementing pagination
            if (pageIndex.HasValue && pageSize.HasValue)
            {
                // Ensure the pageIndex and pageSize are valid
                int validPageIndex = pageIndex.Value > 0 ? pageIndex.Value - 1 : 0;
                int validPageSize = pageSize.Value > 0 ? pageSize.Value : 10; // Assuming a default pageSize of 10 if an invalid value is passed

                query = query.Skip(validPageIndex * validPageSize).Take(validPageSize);
            }

            return new QueryResultModel<List<ProjectCategoryFilterResultModel>>()
            {
                TotalCount = totalCount,
                Data = query.ToList(),
            };
        }
        public async Task<ProjectCategory> GetProjectCategoryById(Guid id)
        {
            return await _dbContext.ProjectCategory.FirstOrDefaultAsync(x => x.Id == id);

        }
    }
}


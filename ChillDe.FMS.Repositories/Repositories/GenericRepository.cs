using System.Collections.Generic;
using System.Linq.Expressions;
using ChillDe.FMS.Repositories.Common;
using ChillDe.FMS.Repositories.Entities;
using ChillDe.FMS.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ChillDe.FMS.Repositories.Repositories
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : BaseEntity
    {
        protected DbSet<TEntity> _dbSet;
        private readonly IClaimsService _claimsService;

        public GenericRepository(AppDbContext dbContext, IClaimsService claimsService)
        {
            _dbSet = dbContext.Set<TEntity>();
            _claimsService = claimsService;
        }

        public async Task<TEntity?> GetAsync(Guid id)
        {
            var result = await _dbSet.FirstOrDefaultAsync(x => x.Id == id);
            // todo should throw exception when result is not found
            return result;
        }

        public async Task<List<TEntity>> GetAllAsync(
            Expression<Func<TEntity, bool>> filter = null, // Các hàm filter
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, // Các hàm sort
            Expression<Func<TEntity, object>>[] includes = null, // Include các bảng khác nếu cần
            string includeProperties = "", // Chỉ định lấy field nào của object
            int? pageIndex = null,
            int? pageSize = null)
        {
            IQueryable<TEntity> query = _dbSet;

            foreach (var include in includes)
            {
                query = query.Include(include);
            }

            if (filter != null)
            {
                query = query.Where(filter);
            }

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
                int validPageSize =
                    pageSize.Value > 0
                        ? pageSize.Value
                        : PaginationConstant.DEFAULT_MIN_PAGE_SIZE;

                query = query.Skip(validPageIndex * validPageSize).Take(validPageSize);
            }

            return query.ToList();
        }
        public async Task<int> Count(Expression<Func<TEntity, bool>> filter = null)
        {
            IQueryable<TEntity> query = _dbSet;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            return await query.CountAsync();
        }

        public async Task AddAsync(TEntity entity)
        {
            entity.CreationDate = DateTime.UtcNow;
            entity.CreatedBy = _claimsService.GetCurrentUserId;
            await _dbSet.AddAsync(entity);
        }

        public async Task AddRangeAsync(List<TEntity> entities)
        {
            foreach (var entity in entities)
            {
                entity.CreationDate = DateTime.UtcNow;
                entity.CreatedBy = _claimsService.GetCurrentUserId;
            }

            await _dbSet.AddRangeAsync(entities);
        }

        public void Update(TEntity entity)
        {
            entity.ModificationDate = DateTime.UtcNow;
            entity.ModifiedBy = _claimsService.GetCurrentUserId;
            _dbSet.Update(entity);
        }

        public void UpdateRange(List<TEntity> entities)
        {
            foreach (var entity in entities)
            {
                entity.ModificationDate = DateTime.UtcNow;
                entity.ModifiedBy = _claimsService.GetCurrentUserId;
            }

            _dbSet.UpdateRange(entities);
        }

        public void SoftDelete(TEntity entity)
        {
            entity.IsDeleted = true;
            entity.DeletionDate = DateTime.UtcNow;
            entity.DeletedBy = _claimsService.GetCurrentUserId;
            _dbSet.Update(entity);
        }

        public void SoftDeleteRange(List<TEntity> entities)
        {
            foreach (var entity in entities)
            {
                entity.IsDeleted = true;
                entity.DeletionDate = DateTime.UtcNow;
                entity.DeletedBy = _claimsService.GetCurrentUserId;
            }

            _dbSet.UpdateRange(entities);
        }

        public void Restore(TEntity entity)
        {
            entity.IsDeleted = false;
            entity.DeletionDate = null;
            entity.DeletedBy = null;
            entity.ModificationDate = DateTime.UtcNow;
            entity.ModifiedBy = _claimsService.GetCurrentUserId;
            _dbSet.Update(entity);
        }

        public void RestoreRange(List<TEntity> entities)
        {
            foreach (var entity in entities)
            {
                entity.IsDeleted = false;
                entity.DeletionDate = null;
                entity.DeletedBy = null;
                entity.ModificationDate = DateTime.UtcNow;
                entity.ModifiedBy = _claimsService.GetCurrentUserId;
            }

            _dbSet.UpdateRange(entities);
        }

        public void HardDelete(TEntity entity)
        {
            _dbSet.Remove(entity);
        }

        public void HardDeleteRange(List<TEntity> entities)
        {
            _dbSet.RemoveRange(entities);
        }
    }
}
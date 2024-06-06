using System.Linq.Expressions;
using ChillDe.FMS.Repositories.Entities;
using ChillDe.FMS.Repositories.Models.QueryModels;

namespace ChillDe.FMS.Repositories.Interfaces
{
    public interface IGenericRepository<TEntity> where TEntity : BaseEntity
    {
        Task<TEntity?> GetAsync(Guid id);

        Task<QueryResultModel<List<TEntity>>> GetAllAsync(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            Expression<Func<TEntity, object>>[] includes = null,
            string includeProperties = "",
            int? pageIndex = null,
            int? pageSize = null
        );

        Task AddAsync(TEntity entity);
        Task AddRangeAsync(List<TEntity> entities);
        void Update(TEntity entity);
        void UpdateRange(List<TEntity> entities);
        void SoftDelete(TEntity entity);
        void SoftDeleteRange(List<TEntity> entities);
        void Restore(TEntity entity);
        void RestoreRange(List<TEntity> entities);
        void HardDelete(TEntity entity);
        void HardDeleteRange(List<TEntity> entities);
    }
}
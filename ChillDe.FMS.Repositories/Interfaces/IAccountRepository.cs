using System.Linq.Expressions;
using ChillDe.FMS.Repositories.Entities;
using ChillDe.FMS.Repositories.Models.AccountModels;
using ChillDe.FMS.Repositories.Models.QueryModels;

namespace ChillDe.FMS.Repositories.Interfaces
{
    public interface IAccountRepository
    {
        Task<Account> GetAccountByCode(string code);
        
        Task<QueryResultModel<List<AccountFilterResultModel>>> GetAllAsync(
            Expression<Func<AccountFilterResultModel, bool>> filter = null,
            Func<IQueryable<AccountFilterResultModel>, IOrderedQueryable<AccountFilterResultModel>> orderBy = null,
            string includeProperties = "",
            int? pageIndex = null,
            int? pageSize = null
        );
    }
}
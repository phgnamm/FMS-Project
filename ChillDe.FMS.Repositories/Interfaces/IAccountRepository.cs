using System.Linq.Expressions;
using ChillDe.FMS.Repositories.Entities;
using ChillDe.FMS.Repositories.Models.AccountModels;
using ChillDe.FMS.Repositories.Models.QueryModels;

namespace ChillDe.FMS.Repositories.Interfaces
{
    public interface IAccountRepository
    {
        Task<Account> GetAccountByCode(string code);
        
        Task<QueryResultModel<List<AccountModel>>> GetAllAsync(
            Expression<Func<AccountModel, bool>> filter = null,
            Func<IQueryable<AccountModel>, IOrderedQueryable<AccountModel>> orderBy = null,
            string includeProperties = "",
            int? pageIndex = null,
            int? pageSize = null
        );
    }
}
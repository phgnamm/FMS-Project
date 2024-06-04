using Repositories.Common;
using Repositories.Entities;
using Repositories.ViewModels.AccountModels;

namespace Repositories.Interfaces
{
    public interface IAccountRepository
    {
        Task<Account> GetAccountByCode(string code);

        public Task<List<AccountModel>> GetAccountsByFilter(PaginationParameter paginationParameter,
            AccountFilterModel accountFilterModel);
    }
}
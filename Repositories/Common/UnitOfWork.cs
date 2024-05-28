using Repositories.Interfaces;

namespace Repositories.Common
{
	public class UnitOfWork : IUnitOfWork
	{
		private readonly AppDbContext _dbContext;
		private readonly IAccountRepository _accountRepository;
		private readonly IFreelancerRepository _freelancerRepository;

		public UnitOfWork(AppDbContext dbContext, IAccountRepository accountRepository, IFreelancerRepository freelancerRepository)
		{
			_dbContext = dbContext;
			_accountRepository = accountRepository;
			_freelancerRepository = freelancerRepository;
		}

		public AppDbContext DbContext => _dbContext;
		public IAccountRepository AccountRepository => _accountRepository;
		public IFreelancerRepository FreelancerRepository => _freelancerRepository;

		public async Task<int> SaveChangeAsync()
		{
			return await _dbContext.SaveChangesAsync();
		}
	}
}

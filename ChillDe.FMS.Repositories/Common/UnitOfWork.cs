using ChillDe.FMS.Repositories.Interfaces;

namespace ChillDe.FMS.Repositories.Common
{
	public class UnitOfWork : IUnitOfWork
	{
		private readonly AppDbContext _dbContext;
		private readonly IAccountRepository _accountRepository;
		private readonly IFreelancerRepository _freelancerRepository;
		private readonly IProjectRepository _projectRepository;
		private readonly ISkillRepository _skillRepository;
		private readonly IFreelancerSkillRepository _freelancerSkillRepository;

		public UnitOfWork(AppDbContext dbContext, IAccountRepository accountRepository, IFreelancerRepository freelancerRepository, IProjectRepository projectRepository, ISkillRepository skillRepository, IFreelancerSkillRepository freelancerSkillRepository)
		{
			_dbContext = dbContext;
			_accountRepository = accountRepository;
			_freelancerRepository = freelancerRepository;
			_projectRepository = projectRepository;
			_skillRepository = skillRepository;
			_freelancerSkillRepository = freelancerSkillRepository;
		}

		public AppDbContext DbContext => _dbContext;
		public IAccountRepository AccountRepository => _accountRepository;
		public IFreelancerRepository FreelancerRepository => _freelancerRepository;
		public IProjectRepository ProjectRepository => _projectRepository;
		public ISkillRepository SkillRepository => _skillRepository;
		public IFreelancerSkillRepository FreelancerSkillRepository => _freelancerSkillRepository;

		public async Task<int> SaveChangeAsync()
		{
			return await _dbContext.SaveChangesAsync();
		}
	}
}

using ChillDe.FMS.Repositories.Interfaces;
using Repositories.Interfaces;

namespace ChillDe.FMS.Repositories.Common
{
	public class UnitOfWork : IUnitOfWork
	{
		private readonly AppDbContext _dbContext;
		private readonly IAccountRepository _accountRepository;
		private readonly IFreelancerRepository _freelancerRepository;
		private readonly IProjectRepository _projectRepository;
		private readonly ISkillRepository _skillRepository;
		private readonly IProjectDeliverableRepository _projectDeliverableRepository;
        private readonly IDeliverableTypeRepository _deliverableTypeRepository;
        private readonly IProjectCategoryReposioty _projectCategoryReposioty;
		private readonly IFreelancerSkillRepository _freelancerSkillRepository;

        public UnitOfWork(AppDbContext dbContext, IAccountRepository accountRepository, 
			IFreelancerRepository freelancerRepository, IProjectRepository projectRepository, 
			ISkillRepository skillRepository, IProjectDeliverableRepository projectDeliverableRepository,
			IDeliverableTypeRepository deliverableTypeRepository, IProjectCategoryReposioty projectCategoryReposioty,
			IFreelancerSkillRepository freelancerSkillRepository)
		{
			_dbContext = dbContext;
			_accountRepository = accountRepository;
			_freelancerRepository = freelancerRepository;
			_projectRepository = projectRepository;
			_skillRepository = skillRepository;
			_projectDeliverableRepository = projectDeliverableRepository;
			_deliverableTypeRepository = deliverableTypeRepository;
			_projectCategoryReposioty = projectCategoryReposioty;
			_freelancerSkillRepository = freelancerSkillRepository;
		}

		public AppDbContext DbContext => _dbContext;
		public IAccountRepository AccountRepository => _accountRepository;
		public IFreelancerRepository FreelancerRepository => _freelancerRepository;
		public IProjectRepository ProjectRepository => _projectRepository;
		public ISkillRepository SkillRepository => _skillRepository;
        public IProjectDeliverableRepository ProjectDeliverableRepository => _projectDeliverableRepository;
        public IDeliverableTypeRepository DeliverableTypeRepository => _deliverableTypeRepository;
        public IProjectCategoryReposioty ProjectCategoryReposioty => _projectCategoryReposioty;

		public IFreelancerSkillRepository FreelancerSkillRepository => _freelancerSkillRepository;
        public async Task<int> SaveChangeAsync()
		{
			return await _dbContext.SaveChangesAsync();
		}
	}
}

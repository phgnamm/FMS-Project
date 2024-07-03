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
        private readonly IProjectCategoryRepository _projectCategoryReposioty;
		private readonly IFreelancerSkillRepository _freelancerSkillRepository;
        private readonly IProjectApplyRepository _projectApplyRepository;
        private readonly IDeliverableProductRepository _deliverableProductRepository;
		private readonly ITransactionRepository _transactionRepository;
		private readonly IDashboardRepository _dashboardRepository;


        public UnitOfWork(AppDbContext dbContext, IAccountRepository accountRepository, 
			IFreelancerRepository freelancerRepository, IProjectRepository projectRepository, 
			ISkillRepository skillRepository, IProjectDeliverableRepository projectDeliverableRepository,
			IDeliverableTypeRepository deliverableTypeRepository, 
			IProjectCategoryRepository projectCategoryReposioty,
			IFreelancerSkillRepository freelancerSkillRepository, 
			IProjectApplyRepository projectApplyRepository, 
			IDeliverableProductRepository deliverableProductRepository,
			ITransactionRepository transactionRepository,
			IDashboardRepository dashboardRepository
			)
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
			_projectApplyRepository = projectApplyRepository;
			_deliverableProductRepository = deliverableProductRepository;
			_transactionRepository = transactionRepository;
			_dashboardRepository = dashboardRepository;
		}

		public AppDbContext DbContext => _dbContext;
		public IAccountRepository AccountRepository => _accountRepository;
		public IFreelancerRepository FreelancerRepository => _freelancerRepository;
		public IProjectRepository ProjectRepository => _projectRepository;
		public ISkillRepository SkillRepository => _skillRepository;
        public IProjectDeliverableRepository ProjectDeliverableRepository => _projectDeliverableRepository;
        public IDeliverableTypeRepository DeliverableTypeRepository => _deliverableTypeRepository;
        public IProjectCategoryRepository ProjectCategoryReposioty => _projectCategoryReposioty;
		public IProjectApplyRepository ProjectApplyRepository => _projectApplyRepository;
		public IFreelancerSkillRepository FreelancerSkillRepository => _freelancerSkillRepository;
        public IDeliverableProductRepository DeliverableProductRepository => _deliverableProductRepository;
		public ITransactionRepository TransactionRepository => _transactionRepository;
		public IDashboardRepository DashboardRepository => _dashboardRepository;

        public async Task<int> SaveChangeAsync()
		{
			return await _dbContext.SaveChangesAsync();
		}
	}
}

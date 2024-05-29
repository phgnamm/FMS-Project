﻿using Repositories.Interfaces;

namespace Repositories.Common
{
	public class UnitOfWork : IUnitOfWork
	{
		private readonly AppDbContext _dbContext;
		private readonly IAccountRepository _accountRepository;
		private readonly IFreelancerRepository _freelancerRepository;
		private readonly IProjectRepository _projectRepository;
		private readonly ISkillRepository _skillRepository;

		public UnitOfWork(AppDbContext dbContext, IAccountRepository accountRepository, IFreelancerRepository freelancerRepository, IProjectRepository projectRepository)
		public UnitOfWork(AppDbContext dbContext, IAccountRepository accountRepository, IFreelancerRepository freelancerRepository, ISkillRepository skillRepository)
		{
			_dbContext = dbContext;
			_accountRepository = accountRepository;
			_freelancerRepository = freelancerRepository;
			_projectRepository = projectRepository;
			_skillRepository = skillRepository;
		}

		public AppDbContext DbContext => _dbContext;
		public IAccountRepository AccountRepository => _accountRepository;
		public IFreelancerRepository FreelancerRepository => _freelancerRepository;
		public IProjectRepository ProjectRepository => _projectRepository;
		public ISkillRepository SkillRepository => _skillRepository;

		public async Task<int> SaveChangeAsync()
		{
			return await _dbContext.SaveChangesAsync();
		}
	}
}

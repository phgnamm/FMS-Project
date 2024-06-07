namespace ChillDe.FMS.Repositories.Interfaces
{
    public interface IUnitOfWork
    {
        AppDbContext DbContext { get; }
        IAccountRepository AccountRepository { get; }
        IFreelancerRepository FreelancerRepository { get; }
        IProjectRepository ProjectRepository { get; }
        ISkillRepository SkillRepository { get; }
        IFreelancerSkillRepository FreelancerSkillRepository { get; }

		public Task<int> SaveChangeAsync();
	}
}

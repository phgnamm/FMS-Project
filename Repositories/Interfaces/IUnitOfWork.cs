namespace Repositories.Interfaces
{
    public interface IUnitOfWork
    {
        AppDbContext DbContext { get; }
        IAccountRepository AccountRepository { get; }
        IFreelancerRepository FreelancerRepository { get; }
        IProjectRepository ProjectRepository { get; }
        ISkillRepository SkillRepository { get; }
        IProjectDeliverableRepository ProjectDeliverableRepository { get; }
        IDeliverableTypeRepository DeliverableTypeRepository { get; }
        IProjectCategoryReposioty ProjectCategoryReposioty { get; }

        public Task<int> SaveChangeAsync();
	}
}

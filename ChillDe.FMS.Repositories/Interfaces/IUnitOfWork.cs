using ChillDe.FMS.Repositories.Repositories;
using Repositories.Interfaces;

namespace ChillDe.FMS.Repositories.Interfaces
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
        IFreelancerSkillRepository FreelancerSkillRepository {  get; }

        public Task<int> SaveChangeAsync();
	}
}

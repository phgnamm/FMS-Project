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
        IProjectCategoryRepository ProjectCategoryReposioty { get; }
        IProjectApplyRepository ProjectApplyRepository { get; }
        IFreelancerSkillRepository FreelancerSkillRepository {  get; }
        IDeliverableProductRepository DeliverableProductRepository { get; }

        public Task<int> SaveChangeAsync();
	}
}

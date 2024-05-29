namespace Repositories.Interfaces
{
    public interface IUnitOfWork
    {
        AppDbContext DbContext { get; }
        IAccountRepository AccountRepository { get; }
        IFreelancerRepository FreelancerRepository { get; }
        IProjectRepository ProjectRepository { get; }

        public Task<int> SaveChangeAsync();
    }
}
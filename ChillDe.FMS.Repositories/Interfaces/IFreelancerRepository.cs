using ChillDe.FMS.Repositories.Entities;

namespace ChillDe.FMS.Repositories.Interfaces;

public interface IFreelancerRepository : IGenericRepository<Freelancer>
{
    Task<Freelancer> GetFreelancerByEmail(string email);
    Task<Freelancer> GetFreelancerById(Guid id);
}
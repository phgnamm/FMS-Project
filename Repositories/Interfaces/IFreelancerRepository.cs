using AutoMapper;
using Repositories.Entities;

namespace Repositories.Interfaces;

public interface IFreelancerRepository : IGenericRepository<Freelancer>
{
    Task<Freelancer> GetFreelancerByEmail(string email);
    Task<Freelancer> GetFreelancerById(Guid id);
}
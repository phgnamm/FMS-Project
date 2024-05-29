using AutoMapper;
using Repositories.Common;
using Repositories.Entities;
using Repositories.ViewModels.FreelancerModels;

namespace Repositories.Interfaces;

public interface IFreelancerRepository : IGenericRepository<Freelancer>
{
    Task<Freelancer> GetFreelancerByEmail(string email);
    Task<List<FreelancerDetailModel>> GetFreelancersByFilter(PaginationParameter paginationParameter,
          FreelancerFilterModel freelancerFilterModel);
}
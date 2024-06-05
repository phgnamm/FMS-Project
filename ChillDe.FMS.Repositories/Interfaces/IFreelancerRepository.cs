using AutoMapper;
using ChillDe.FMS.Repositories.Common;
using ChillDe.FMS.Repositories.Entities;

namespace ChillDe.FMS.Repositories.Interfaces;

public interface IFreelancerRepository : IGenericRepository<Freelancer>
{
    Task<Freelancer> GetFreelancerByEmail(string email);
    // Task<List<FreelancerDetailModel>> GetFreelancersByFilter(PaginationParameter paginationParameter,
    //       FreelancerFilterModel freelancerFilterModel);
}
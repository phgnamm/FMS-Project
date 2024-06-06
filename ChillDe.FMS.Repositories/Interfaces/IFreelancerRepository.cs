using AutoMapper;
using ChillDe.FMS.Repositories.Common;
using ChillDe.FMS.Repositories.Entities;
using ChillDe.FMS.Repositories.ViewModels.FreelancerModels;
using System.Linq.Expressions;

namespace ChillDe.FMS.Repositories.Interfaces;

public interface IFreelancerRepository : IGenericRepository<Freelancer>
{
    Task<Freelancer> GetFreelancerByEmail(string email);
    Task<List<FreelancerDetailModel>> GetFreelancersByFilter(
     Expression<Func<FreelancerDetailModel, bool>> filter = null,
     Func<IQueryable<FreelancerDetailModel>, IOrderedQueryable<FreelancerDetailModel>> orderBy = null,
     string includeProperties = "",
     int? pageIndex = null,
     int? pageSize = null);
}
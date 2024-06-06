using ChillDe.FMS.Repositories.Common;
using ChillDe.FMS.Repositories.Entities;
using ChillDe.FMS.Repositories.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ChillDe.FMS.Repositories.Enums;
using System.Linq.Expressions;
using ChillDe.FMS.Repositories.ViewModels.FreelancerModels;

namespace ChillDe.FMS.Repositories.Repositories;

public class FreelancerRepository : GenericRepository<Freelancer>, IFreelancerRepository
{
    private readonly AppDbContext _dbContext;

    public FreelancerRepository(AppDbContext dbContext, IClaimsService claimsService) : base(dbContext, claimsService)
    {
        _dbContext = dbContext;
    }

    public async Task<Freelancer> GetFreelancerByEmail(string email)
    {
        return await _dbContext.Freelancer.FirstOrDefaultAsync(x => x.Email == email);
    }

}
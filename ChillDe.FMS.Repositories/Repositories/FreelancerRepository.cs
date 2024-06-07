﻿using ChillDe.FMS.Repositories.Entities;
using ChillDe.FMS.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;


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

    public async Task<Freelancer?> GetFreelancerById(Guid id)
    {
        return await _dbContext.Freelancer
            .Include(f => f.FreelancerSkills)
            .ThenInclude(fs => fs.Skill)
            .FirstOrDefaultAsync(f => f.Id == id);
    }


}
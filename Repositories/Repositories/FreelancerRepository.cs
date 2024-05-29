using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Repositories.Common;
using Repositories.Entities;
using Repositories.Enums;
using Repositories.Interfaces;
using Repositories.ViewModels.AccountModels;
using Repositories.ViewModels.FreelancerModels;

namespace Repositories.Repositories;

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

    public async Task<List<FreelancerDetailModel>> GetFreelancersByFilter(PaginationParameter paginationParameter,
     FreelancerFilterModel freelancerFilterModel)
    {
        var freelancerList = _dbContext.Freelancer
            .Include(f => f.FreelancerSkills)
            .ThenInclude(fs => fs.Skill)
            .AsQueryable();

        // Filter
        if (freelancerFilterModel.Status.HasValue)
        {
            freelancerList = freelancerList.Where(x => x.Status == freelancerFilterModel.Status.ToString());
        }

        if (freelancerFilterModel.Gender != null)   
        {
            freelancerList = freelancerList.Where(x => x.Gender == freelancerFilterModel.Gender);
        }

        if (freelancerFilterModel.SkillName != null)
        {
            freelancerList = freelancerList.Where(f => f.FreelancerSkills.Any(fs => fs.Skill.Name == freelancerFilterModel.SkillName));
        }

        if (freelancerFilterModel.SkillType != null)
        {
            freelancerList = freelancerList.Where(f => f.FreelancerSkills.Any(fs => fs.Skill.Type == freelancerFilterModel.SkillType));
        }

        // Search
        if (freelancerFilterModel.Search != null)
        {
            freelancerList = freelancerList.Where(x =>
                x.FirstName.ToLower().Contains(freelancerFilterModel.Search.ToLower()) ||
                x.LastName.ToLower().Contains(freelancerFilterModel.Search.ToLower()) ||
                x.Code.ToLower().Contains(freelancerFilterModel.Search.ToLower()) ||
                x.Email.ToLower().Contains(freelancerFilterModel.Search.ToLower()));
        }

        switch (freelancerFilterModel.Sort?.ToLower())
        {
            case "firstname":
                freelancerList = freelancerFilterModel.SortDirection.ToLower() == "asc"
                    ? freelancerList.OrderBy(x => x.FirstName)
                    : freelancerList.OrderByDescending(x => x.FirstName);
                break;
            case "lastname":
                freelancerList = freelancerFilterModel.SortDirection.ToLower() == "asc"
                    ? freelancerList.OrderBy(x => x.LastName)
                    : freelancerList.OrderByDescending(x => x.LastName);
                break;
            case "code":
                freelancerList = freelancerFilterModel.SortDirection.ToLower() == "asc"
                    ? freelancerList.OrderBy(x => x.Code)
                    : freelancerList.OrderByDescending(x => x.Code);
                break;
            case "dateofbirth":
                freelancerList = freelancerFilterModel.SortDirection.ToLower() == "asc"
                    ? freelancerList.OrderBy(x => x.DateOfBirth)
                    : freelancerList.OrderByDescending(x => x.DateOfBirth);
                break;
            default:
                freelancerList = freelancerList.OrderByDescending(x => x.CreationDate);
                break;
        }
        var freelancerModelList = await freelancerList
          .Select(f => new FreelancerDetailModel
          {
              FirstName = f.FirstName,
              LastName = f.LastName,
              Gender = f.Gender.ToString(),
              DateOfBirth = (DateTime)f.DateOfBirth,
              Address = f.Address,
              Image = f.Image,
              Code = f.Code,
              Email = f.Email,
              PhoneNumber = f.PhoneNumber,
              Skills = f.FreelancerSkills
                          .GroupBy(fs => fs.Skill.Type)
                          .Select(group => new SkillSet
                          {
                              SkillType = group.Key,
                              SkillNames = group.Select(fs => fs.Skill.Name).ToList()
                          })
                          .ToList()
          })
          .ToListAsync();

        return freelancerModelList;
    }
}
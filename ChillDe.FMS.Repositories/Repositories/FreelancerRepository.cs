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

    // public async Task<List<FreelancerDetailModel>> GetFreelancersByFilter(PaginationParameter paginationParameter,
    //  FreelancerFilterModel freelancerFilterModel)
    // {
    //     var freelancerList = _dbContext.Freelancer
    //         .Include(f => f.FreelancerSkills)
    //         .ThenInclude(fs => fs.Skill)
    //         .AsQueryable();
    //
    //     // Filter
    //     if (freelancerFilterModel.Status.HasValue)
    //     {
    //         freelancerList = freelancerList.Where(x => x.Status == freelancerFilterModel.Status.ToString());
    //     }
    //
    //     if (freelancerFilterModel.Gender != null)   
    //     {
    //         freelancerList = freelancerList.Where(x => x.Gender == freelancerFilterModel.Gender);
    //     }
    //
    //     if (freelancerFilterModel.SkillName != null)
    //     {
    //         freelancerList = freelancerList.Where(f => f.FreelancerSkills.Any(fs => fs.Skill.Name == freelancerFilterModel.SkillName));
    //     }
    //
    //     if (freelancerFilterModel.SkillType != null)
    //     {
    //         freelancerList = freelancerList.Where(f => f.FreelancerSkills.Any(fs => fs.Skill.Type == freelancerFilterModel.SkillType));
    //     }
    //
    //     // Search
    //     if (freelancerFilterModel.Search != null)
    //     {
    //         freelancerList = freelancerList.Where(x =>
    //             x.FirstName.ToLower().Contains(freelancerFilterModel.Search.ToLower()) ||
    //             x.LastName.ToLower().Contains(freelancerFilterModel.Search.ToLower()) ||
    //             x.Code.ToLower().Contains(freelancerFilterModel.Search.ToLower()) ||
    //             x.Email.ToLower().Contains(freelancerFilterModel.Search.ToLower()));
    //     }
    //
    //     switch (freelancerFilterModel.Sort?.ToLower())
    //     {
    //         case "firstname":
    //             freelancerList = freelancerFilterModel.SortDirection.ToLower() == "asc"
    //                 ? freelancerList.OrderBy(x => x.FirstName)
    //                 : freelancerList.OrderByDescending(x => x.FirstName);
    //             break;
    //         case "lastname":
    //             freelancerList = freelancerFilterModel.SortDirection.ToLower() == "asc"
    //                 ? freelancerList.OrderBy(x => x.LastName)
    //                 : freelancerList.OrderByDescending(x => x.LastName);
    //             break;
    //         case "code":
    //             freelancerList = freelancerFilterModel.SortDirection.ToLower() == "asc"
    //                 ? freelancerList.OrderBy(x => x.Code)
    //                 : freelancerList.OrderByDescending(x => x.Code);
    //             break;
    //         case "dateofbirth":
    //             freelancerList = freelancerFilterModel.SortDirection.ToLower() == "asc"
    //                 ? freelancerList.OrderBy(x => x.DateOfBirth)
    //                 : freelancerList.OrderByDescending(x => x.DateOfBirth);
    //             break;
    //         default:
    //             freelancerList = freelancerList.OrderByDescending(x => x.CreationDate);
    //             break;
    //     }
    //     var freelancerModelList = await freelancerList
    //       .Select(f => new FreelancerDetailModel
    //       {
    //           FirstName = f.FirstName,
    //           LastName = f.LastName,
    //           Gender = f.Gender.ToString(),
    //           DateOfBirth = (DateTime)f.DateOfBirth,
    //           Address = f.Address,
    //           Image = f.Image,
    //           Code = f.Code,
    //           Email = f.Email,
    //           PhoneNumber = f.PhoneNumber,
    //           Skills = f.FreelancerSkills
    //                       .GroupBy(fs => fs.Skill.Type)
    //                       .Select(group => new SkillSet
    //                       {
    //                           SkillType = group.Key,
    //                           SkillNames = group.Select(fs => fs.Skill.Name).ToList()
    //                       })
    //                       .ToList()
    //       })
    //       .ToListAsync();
    //
    //     return freelancerModelList;
    // }

    public async Task<List<FreelancerDetailModel>> GetFreelancersByFilter(
    Expression<Func<FreelancerDetailModel, bool>> filter = null,
    Func<IQueryable<FreelancerDetailModel>, IOrderedQueryable<FreelancerDetailModel>> orderBy = null,
    string includeProperties = "",
    int? pageIndex = null,
    int? pageSize = null)
    {
        IQueryable<FreelancerDetailModel> query =
            from freelancer in _dbContext.Freelancer
            join freelancerSkill in _dbContext.FreelancerSkill on freelancer.Id equals freelancerSkill.FreelancerId
            join skill in _dbContext.Skill on freelancerSkill.SkillId equals skill.Id
            select new FreelancerDetailModel
            {
                Id = freelancer.Id,
                FirstName = freelancer.FirstName,
                LastName = freelancer.LastName,
                Gender = freelancer.Gender,
                DateOfBirth = freelancer.DateOfBirth,
                Address = freelancer.Address,
                Image = freelancer.Image,
                Code = freelancer.Code,
                Email = freelancer.Email,
                PhoneNumber = freelancer.PhoneNumber,
                Status = freelancer.Status,
                Wallet = freelancer.Wallet,
                CreationDate = freelancer.CreationDate,
                Skills = new List<SkillSet>
                             {
                                 new SkillSet
                                 {
                                     SkillType = skill.Type,
                                     SkillNames = new List<string> { skill.Name }
                                 }
                             }
            };
        // Apply filter
        if (filter != null)
        {
            query = query.Where(filter);
        }
        // Apply includes
        foreach (var includeProperty in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
        {
            query = query.Include(includeProperty);
        }
        // Apply order by
        if (orderBy != null)
        {
            query = orderBy(query);
        }
        // Implement pagination
        if (pageIndex.HasValue && pageSize.HasValue)
        {
            int validPageIndex = pageIndex.Value > 0 ? pageIndex.Value - 1 : 0;
            int validPageSize = pageSize.Value > 0 ? pageSize.Value : PaginationConstant.DEFAULT_MIN_PAGE_SIZE;
            query = query.Skip(validPageIndex * validPageSize).Take(validPageSize);
        }
        return await query.ToListAsync();
    }

}
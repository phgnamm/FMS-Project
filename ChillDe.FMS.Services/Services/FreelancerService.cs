using AutoMapper;
using ChillDe.FMS.Repositories.Common;
using ChillDe.FMS.Repositories.Entities;
using ChillDe.FMS.Repositories.Enums;
using ChillDe.FMS.Repositories.Interfaces;
using ChillDe.FMS.Repositories.ViewModels.FreelancerModels;
using ChillDe.FMS.Repositories.ViewModels.ResponseModels;
using Services.Interfaces;
using System.Linq.Expressions;
using System.Linq;

namespace ChillDe.FMS.Services;

public class FreelancerService : IFreelancerService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IClaimsService _claimsService;

    public FreelancerService(IUnitOfWork unitOfWork, IMapper mapper, IClaimsService claimsService)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _claimsService = claimsService;
    }

    public async Task<ResponseDataModel<FreelancerModel>> GetFreelancer(Guid id)
    {
        var user = await _unitOfWork.FreelancerRepository.GetAsync(id);

        if (user == null)
        {
            return new ResponseDataModel<FreelancerModel>()
            {
                Status = false,
                Message = "User not found"
            };
        }

        var userModel = _mapper.Map<FreelancerModel>(user);

        return new ResponseDataModel<FreelancerModel>()
        {
            Status = true,
            Message = "Get account successfully",
            Data = userModel
        };
    }

    public async Task<Pagination<FreelancerDetailModel>> GetFreelancersByFilter(FreelancerFilterModel freelancerFilterModel)
    {
        var freelancerList = await _unitOfWork.FreelancerRepository.GetAllAsync(
            filter: x =>
                x.Status == freelancerFilterModel.Status &&
                (freelancerFilterModel.Gender == null || x.Gender == freelancerFilterModel.Gender) &&
                (freelancerFilterModel.SkillType == null || x.FreelancerSkills.Any(fs => fs.Skill.Type == freelancerFilterModel.SkillType)) &&
                (freelancerFilterModel.SkillName == null || x.FreelancerSkills.Any(fs => fs.Skill.Name.Contains(freelancerFilterModel.SkillName))) &&
                (string.IsNullOrEmpty(freelancerFilterModel.Search) ||
                 x.FirstName.ToLower().Contains(freelancerFilterModel.Search.ToLower()) ||
                 x.LastName.ToLower().Contains(freelancerFilterModel.Search.ToLower()) ||
                 x.Code.ToLower().Contains(freelancerFilterModel.Search.ToLower()) ||
                 x.Email.ToLower().Contains(freelancerFilterModel.Search.ToLower())),
            orderBy: x =>
            {
                switch (freelancerFilterModel.Order.ToLower())
                {
                    case "first-name":
                        return freelancerFilterModel.OrderByDescending
                            ? x.OrderByDescending(x => x.FirstName)
                            : x.OrderBy(x => x.FirstName);
                    case "last-name":
                        return freelancerFilterModel.OrderByDescending
                            ? x.OrderByDescending(x => x.LastName)
                            : x.OrderBy(x => x.LastName);
                    case "code":
                        return freelancerFilterModel.OrderByDescending
                            ? x.OrderByDescending(x => x.Code)
                            : x.OrderBy(x => x.Code);
                    case "date-of-birth":
                        return freelancerFilterModel.OrderByDescending
                            ? x.OrderByDescending(x => x.DateOfBirth)
                            : x.OrderBy(x => x.DateOfBirth);
                    default:
                        return x.OrderBy(x => x.CreationDate);
                }
            },
            pageIndex: freelancerFilterModel.PageIndex,
            pageSize: freelancerFilterModel.PageSize,
            includes: new Expression<Func<Freelancer, object>>[]
                         {
                             x => x.FreelancerSkills,
                         },
            includeProperties: "FreelancerSkills.Skill"
        );

        if (freelancerList != null)
        {
            var freelancerDetailList = freelancerList.Data.Select(f => new FreelancerDetailModel
            {
                Id = f.Id,
                FirstName = f.FirstName,
                LastName = f.LastName,
                Gender = f.Gender,
                DateOfBirth = f.DateOfBirth,
                Address = f.Address,
                Image = f.Image,
                Code = f.Code,
                Email = f.Email,
                PhoneNumber = f.PhoneNumber,
                Wallet = f.Wallet,
                Status = f.Status,
                CreationDate = f.CreationDate,
                Skills = f.FreelancerSkills.GroupBy(fs => fs.Skill.Type)
                            .Select(group => new SkillSet
                            {
                                SkillType = group.Key,
                                SkillNames = group.Select(fs => fs.Skill.Name).ToList()
                            }).ToList()
            }).ToList();

            return new Pagination<FreelancerDetailModel>(freelancerDetailList, freelancerList.TotalCount, freelancerFilterModel.PageIndex, freelancerFilterModel.PageSize);
        }

        return null;
    }

    public async Task<FreelancerImportResponseModel> AddRangeFreelancer(List<FreelancerImportModel> freelancers)
    {
        try
        {
            var freelancerImportList = new List<Freelancer>();
            var response = new FreelancerImportResponseModel();
            var existingFreelancer = await _unitOfWork.FreelancerRepository.GetAllAsync(
              includes: new Expression<Func<Freelancer, object>>[]
                            {
                                x => x.FreelancerSkills,
                            },
                includeProperties: "FreelancerSkills.Skill");
            var existingSkills = await _unitOfWork.SkillRepository.GetAllAsync();
            foreach (FreelancerImportModel newFreelancers in freelancers)
            {
                var freelancerChecking = existingFreelancer.Data.FirstOrDefault(x => x.Email == newFreelancers.Email || x.Code == newFreelancers.Code);

                if (freelancerChecking != null)
                {
                    if (response.DuplicatedFreelancer == null)
                    {
                        response.DuplicatedFreelancer = new List<FreelancerImportModel>();
                    }
                    response.DuplicatedFreelancer.Add(newFreelancers);
                }
                else
                {
                    var newFreelancer = new Freelancer
                    {
                        FirstName = newFreelancers.FirstName,
                        LastName = newFreelancers.LastName,
                        Email = newFreelancers.Email,
                        PhoneNumber = newFreelancers.PhoneNumber,
                        DateOfBirth = newFreelancers.DateOfBirth,
                        Gender = newFreelancers.Gender,
                        Code = newFreelancers.Code,
                        Address = newFreelancers.Address,
                        Status = FreelancerStatus.Available,
                        Wallet = newFreelancers.Wallet,
                        CreationDate = DateTime.UtcNow,
                        CreatedBy = _claimsService.GetCurrentUserId,
                    };
                    // Check and add skills
                    foreach (var skillTypeModel in newFreelancers.Skills)
                    {
                        var validSkills = existingSkills.Data
                            .Where(skill => skillTypeModel.SkillNames.Contains(skill.Name) && skill.Type == skillTypeModel.SkillType)
                            .ToList();

                        foreach (var skill in validSkills)
                        {
                            newFreelancer.FreelancerSkills.Add(new FreelancerSkill
                            {
                                SkillId = skill.Id,
                                FreelancerId = newFreelancer.Id
                            });
                        }
                    }
                    freelancerImportList.Add(newFreelancer);
                }
            }
            if (freelancerImportList.Count > 0)
            {
                await _unitOfWork.FreelancerRepository.AddRangeAsync(freelancerImportList);
                await _unitOfWork.SaveChangeAsync();
                if (response.DuplicatedFreelancer != null)
                {
                    return new FreelancerImportResponseModel
                    {
                        DuplicatedFreelancer = response.DuplicatedFreelancer,
                        AddedFreelancer = _mapper.Map<List<FreelancerImportModel>>(freelancerImportList),
                        Message = "These freelancers have been successfully added and some freelancers are existed",
                        Status = true
                    };
                }
                else
                {
                    return new FreelancerImportResponseModel
                    {
                        AddedFreelancer = _mapper.Map<List<FreelancerImportModel>>(freelancerImportList),
                        Message = "These freelancers have been successfully added",
                        Status = true
                    };
                }
            }
            if (response.DuplicatedFreelancer != null)
            {
                response.Message = "Importing process have duplicated problems";
                response.Status = false;
                return response;
            }
        }
        catch (Exception)
        {
            throw;
        }
        throw new NotImplementedException();
    }

    public async Task<ResponseDataModel<FreelancerDetailModel>> UpdateFreelancerAsync(Guid id, FreelancerImportModel updateFreelancer)
    {
        var existingFreelancer = await _unitOfWork.FreelancerRepository.GetAsync(id);
        if (existingFreelancer == null)
        {
            return new ResponseDataModel<FreelancerDetailModel>()
            {
                Status = false,
                Message = "Freelancer not found"
            };
        }

        if (updateFreelancer.Skills != null && updateFreelancer.Skills.Any())
        {
            var skillNames = new HashSet<string>(updateFreelancer.Skills.SelectMany(skill => skill.SkillNames).Distinct(), StringComparer.OrdinalIgnoreCase);
            var validSkills = (await _unitOfWork.SkillRepository.GetAllAsync(skill => skillNames.Contains(skill.Name))).Data;

            existingFreelancer.FreelancerSkills.Clear();

            foreach (var skillTypeModel in updateFreelancer.Skills)
            {
                foreach (var skill in validSkills.Where(skill => skillTypeModel.SkillNames.Contains(skill.Name) && skill.Type == skillTypeModel.SkillType))
                {
                    existingFreelancer.FreelancerSkills.Add(new FreelancerSkill
                    {
                        SkillId = skill.Id,
                        FreelancerId = existingFreelancer.Id
                    });
                }
            }
        }

        _mapper.Map(updateFreelancer, existingFreelancer);
        _unitOfWork.FreelancerRepository.Update(existingFreelancer);
        await _unitOfWork.SaveChangeAsync();
        var result = _mapper.Map<FreelancerDetailModel>(existingFreelancer);
        return result != null
            ? new ResponseDataModel<FreelancerDetailModel>()
            {
                Status = true,
                Message = "Update freelancer successfully",
                Data = result
            }
            : new ResponseDataModel<FreelancerDetailModel>()
            {
                Status = false,
                Message = "Update freelancer fail"
            };
    }

    private bool AreSkillsEqual(List<SkillInputModel> newSkills, ICollection<FreelancerSkill> existingSkills)
    {
        if (newSkills.Count != existingSkills.Count)
        {
            return false;
        }

        foreach (var newSkill in newSkills)
        {
            var existingSkill = existingSkills.FirstOrDefault(skill =>
                skill.Skill.Name == newSkill.SkillNames.FirstOrDefault() && skill.Skill.Type == newSkill.SkillType);

            if (existingSkill == null || !existingSkill.Skill.Name.Equals(newSkill.SkillNames.FirstOrDefault(), StringComparison.OrdinalIgnoreCase))
            {
                return false;
            }
        }
        return true;
    }




    public async Task<ResponseDataModel<List<FreelancerModel>>> DeleteFreelancer(List<Guid> freelancerIds)
    {
        var deleteList = new List<Freelancer>();
        var freelancerStatus = FreelancerStatus.NotAvailable;
        foreach (Guid freelancerId in freelancerIds)
        {
            var freelancer = await _unitOfWork.FreelancerRepository.GetAsync(freelancerId);
            if (freelancer != null)
            {
                freelancer.Status = freelancerStatus;
                deleteList.Add(freelancer);
            }
        }
        if (deleteList.Count > 0)
        {
            _unitOfWork.FreelancerRepository.SoftDeleteRange(deleteList);
            await _unitOfWork.SaveChangeAsync();
            var result = _mapper.Map<List<FreelancerModel>>(deleteList);
            return new ResponseDataModel<List<FreelancerModel>>()
            {
                Status = true,
                Message = "Delete freelancer successfully",
                Data = result
            };
        }
        return new ResponseDataModel<List<FreelancerModel>>()
        {
            Status = false,
            Message = "Delete freelancer failed"
        };
    }


}
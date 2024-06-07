using AutoMapper;
using ChillDe.FMS.Repositories.Common;
using ChillDe.FMS.Repositories.Entities;
using ChillDe.FMS.Repositories.Enums;
using ChillDe.FMS.Repositories.Interfaces;
using ChillDe.FMS.Repositories.ViewModels.FreelancerModels;
using ChillDe.FMS.Repositories.ViewModels.ResponseModels;
using ChillDe.FMS.Services.ViewModels.FreelancerModels;
using Services.Interfaces;

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

    public async Task<ResponseDataModel<FreelancerDetailModel>> GetFreelancer(Guid id)
    {
        var user = await _unitOfWork.FreelancerRepository.GetFreelancerById(id);

        if (user == null)
        {
            return new ResponseDataModel<FreelancerDetailModel>()
            {
                Status = false,
                Message = "User not found"
            };
        }

        var userModel = _mapper.Map<FreelancerDetailModel>(user);

        return new ResponseDataModel<FreelancerDetailModel>()
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
        var existingFreelancer = await _unitOfWork.FreelancerRepository.GetFreelancerById(id);
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
            var skillNames = new HashSet<string>(updateFreelancer.Skills.SelectMany(skill => skill.SkillNames).Distinct());
            var validSkills = (await _unitOfWork.SkillRepository.GetAllAsync(skill => skillNames.Contains(skill.Name))).Data;
            var freelancerSkillsToAdd = new List<FreelancerSkill>();
            foreach (var skillTypeModel in updateFreelancer.Skills)
            {
                foreach (var skillName in skillTypeModel.SkillNames)
                {
                    var existingSkill = validSkills.FirstOrDefault(skill =>
                        skill.Name.Equals(skillName, StringComparison.OrdinalIgnoreCase) && skill.Type == skillTypeModel.SkillType);

                    if (existingSkill != null)
                    {
                        var existingFreelancerSkill = existingFreelancer.FreelancerSkills.FirstOrDefault(fs =>
                            fs.SkillId == existingSkill.Id && fs.FreelancerId == existingFreelancer.Id);

                        if (existingFreelancerSkill == null)
                        {
                            freelancerSkillsToAdd.Add(new FreelancerSkill
                            {
                                SkillId = existingSkill.Id,
                                FreelancerId = existingFreelancer.Id
                            });
                        }
                    }
                }
            }

            foreach (var skill in freelancerSkillsToAdd)
            {
                existingFreelancer.FreelancerSkills.Add(skill);
            }
        }
        _mapper.Map(updateFreelancer, existingFreelancer);

        try
        {
            _unitOfWork.FreelancerRepository.Update(existingFreelancer);
            await _unitOfWork.SaveChangeAsync();

            var result = _mapper.Map<FreelancerDetailModel>(existingFreelancer);
            return new ResponseDataModel<FreelancerDetailModel>()
            {
                Status = true,
                Message = "Update freelancer successfully",
                Data = result
            };
        }
        catch (Exception ex)
        {
            return new ResponseDataModel<FreelancerDetailModel>()
            {
                Status = false,
                Message = $"Update freelancer fail. Error: {ex.Message}"
            };
        }
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

    public async Task<ResponseModel> RestoreFreelancer(Guid id)
    {
        var freelancer = await _unitOfWork.FreelancerRepository.GetAsync(id);

        if (freelancer == null)
        {
            return new ResponseModel()
            {
                Status = false,
                Message = "Freelancer not found"
            };
        }

        freelancer.IsDeleted = false;
        freelancer.DeletionDate = null;
        freelancer.Status = FreelancerStatus.Available;
        freelancer.DeletedBy = null;
        freelancer.ModificationDate = DateTime.UtcNow;
        freelancer.ModifiedBy = _claimsService.GetCurrentUserId;

        _unitOfWork.FreelancerRepository.Update(freelancer);
        try
        {
            await _unitOfWork.SaveChangeAsync();
            return new ResponseModel
            {
                Status = true,
                Message = "Restore Freelancer successfully",
            };
        }catch (Exception ex)
        {
            return new ResponseModel
            {
                Status = false,
                Message = "Cannot restore Freelancer",
            };
        }
    }
}
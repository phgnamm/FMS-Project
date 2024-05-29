using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Repositories.Entities;
using Repositories.Enums;
using Repositories.Interfaces;
using Repositories.ViewModels.FreelancerModels;
using Repositories.ViewModels.ResponseModels;
using Services.Interfaces;

namespace Services.Services;

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

    public async Task<FreelancerImportResponseModel> AddRangeFreelancer(List<FreelancerImportModel> freelancers)
    {
        try
        {
            var freelancerImportList = new List<Freelancer>();
            var response = new FreelancerImportResponseModel();
            var existingFreelancer = await _unitOfWork.FreelancerRepository.GetAllAsync();
            var existingSkills = await _unitOfWork.SkillRepository.GetAllAsync();
            foreach (FreelancerImportModel newFreelancers in freelancers)
            {
                var freelancerChecking = existingFreelancer.FirstOrDefault(x => x.Email == newFreelancers.Email || x.Code == newFreelancers.Code);

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
                        Status = FreelancerStatus.Available.ToString(),
                        CreationDate = DateTime.UtcNow,
                        CreatedBy = _claimsService.GetCurrentUserId,
                    };
                    // Check and add skills
                    if (newFreelancers.Skill != null && newFreelancers.Skill.Any())
                    {
                        var validSkills = existingSkills.Where(skill => newFreelancers.Skill.Contains(skill.Name)).ToList();
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

    public async Task<ResponseDataModel<FreelancerModel>> UpdateFreelancerAsync(Guid id, FreelancerImportModel updateFreelancer)
    {
        var existingFreelancer = await _unitOfWork.FreelancerRepository.GetAsync(id);
        if (existingFreelancer != null)
        {
            //cần fe khóa cứng cột Code lại
            existingFreelancer = _mapper.Map(updateFreelancer, existingFreelancer);
            _unitOfWork.FreelancerRepository.Update(existingFreelancer);
            await _unitOfWork.SaveChangeAsync();
            var result = _mapper.Map<FreelancerModel>(existingFreelancer);
            if (result != null)
            {
                return new ResponseDataModel<FreelancerModel>()
                {
                    Status = true,
                    Message = "Update freelancer successfully",
                    Data = result
                };
            }
            return new ResponseDataModel<FreelancerModel>()
            {
                Status = false,
                Message = "Update freelancer fail"
            };
        }
        return new ResponseDataModel<FreelancerModel>()
        {
            Status = false,
            Message = "Freelancer not found"
        };
    }

    public async Task<ResponseDataModel<List<FreelancerModel>>> DeleteFreelancer(List<Guid> freelancerIds)
    {
        var deleteList = new List<Freelancer>();
        foreach (Guid freelancerId in freelancerIds)
        {
            var freelancer = await _unitOfWork.FreelancerRepository.GetAsync(freelancerId);
            if (freelancer != null)
            {
                deleteList.Add(freelancer);
            }
        }
        if (deleteList.Count > 0)
        {
            _unitOfWork.FreelancerRepository.SoftDeleteRange(deleteList);
            await _unitOfWork.SaveChangeAsync();
            var result = _mapper.Map<List<FreelancerModel>>(deleteList);
            if (result != null)
            {
                return new ResponseDataModel<List<FreelancerModel>>()
                {
                    Status = true,
                    Message = "Delete freelancer successfully",
                    Data = result
                };
            }
        }
        return new ResponseDataModel<List<FreelancerModel>>()
        {
            Status = false,
            Message = "Delete freelancer failed"
        };
    }

}
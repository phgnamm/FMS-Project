using AutoMapper;
using ChillDe.FMS.Repositories.Common;
using ChillDe.FMS.Repositories.Entities;
using ChillDe.FMS.Repositories.Interfaces;
using ChillDe.FMS.Repositories.Models.SkillModels;
using ChillDe.FMS.Repositories.ViewModels.ResponseModels;
using ChillDe.FMS.Services.Models.SkillModels;
using Services.Interfaces;

namespace ChillDe.FMS.Services
{
    public class SkillService : ISkillService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public SkillService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ResponseDataModel<SkillModel>> GetSkill(Guid id)
        {
            var skill = await _unitOfWork.SkillRepository.GetAsync(id);
            if (skill == null)
            {
                return new ResponseDataModel<SkillModel>()
                {
                    Status = false,
                    Message = "Skill doesn't exist"
                };
            }

            var result = _mapper.Map<SkillModel>(skill);

            return new ResponseDataModel<SkillModel>()
            {
                Status = true,
                Message = "Get skill successfully",
                Data = result
            };
        }

        public async Task<ResponseModel> CreateSkill(List<SkillCreateModel> skillCreateModel)
        {
           var skillList = _mapper.Map<List<Skill>>(skillCreateModel);
            if(skillList != null) {
                foreach (var skill in skillList)
                {
                    await _unitOfWork.SkillRepository.AddAsync(skill);
                }
                await _unitOfWork.SaveChangeAsync();
                return new ResponseModel()
                {
                    Status = true,
                    Message = "Skill created successfully"
                };
                
            }
            return new ResponseModel()
            {
                Status = false,
                Message = "Can not create skill !"
            };  
        }

        public async Task<ResponseModel> DeleteSkill(Guid id)
        {
            var skill =  await _unitOfWork.SkillRepository.GetSkillById(id);
            if(skill != null)
            {
                skill.IsDeleted = true;
                _unitOfWork.SkillRepository.SoftDelete(skill);
                await _unitOfWork.SaveChangeAsync();
                return new ResponseModel()
                {
                    Status = true,
                    Message = "Skill deleted successfully"

                };     
            }
            return new ResponseModel()
            {
                Status = false,
                Message = "Skill not found"
            };
        }
    

        public async Task<Pagination<SkillModel>> GetAllSkill(SkillFilterModel skillFilterModel)
        {
            var skillList = await _unitOfWork.SkillRepository.GetAllAsync(pageIndex: skillFilterModel.PageIndex,
                pageSize: skillFilterModel.PageSize,
                filter: (x =>
                    x.IsDeleted == skillFilterModel.IsDeleted &&
                    (string.IsNullOrEmpty(skillFilterModel.Search) ||
                     x.Type.ToLower().Contains(skillFilterModel.Search.ToLower()) ||
                     x.Name.ToLower().Contains(skillFilterModel.Search.ToLower()))),
                orderBy: (x =>
                {
                    switch (skillFilterModel.Order.ToLower())
                    {
                        case "name":
                            return skillFilterModel.OrderByDescending
                                ? x.OrderByDescending(x => x.Name)
                                : x.OrderBy(x => x.Name);
                        case "type":
                            return skillFilterModel.OrderByDescending
                                ? x.OrderByDescending(x => x.Type)
                                : x.OrderBy(x => x.Type);
                        default:
                            return skillFilterModel.OrderByDescending
                                ? x.OrderByDescending(x => x.CreationDate)
                                : x.OrderBy(x => x.CreationDate);
                    }
                })
            );

            if (skillList != null)
            {
                var skillModelList = _mapper.Map<List<SkillModel>>(skillList.Data);
                return new Pagination<SkillModel>(skillModelList, skillList.TotalCount, skillFilterModel.PageIndex,
                    skillFilterModel.PageSize);
            }

            return null;
        }
    

        public async Task<Pagination<SkillGroupModel>> GetAllSkillsGroupByType(SkillFilterModel skillFilterModel)
        {
            var skillList = await _unitOfWork.SkillRepository.GetAllAsync(pageIndex: skillFilterModel.PageIndex,
                pageSize: PaginationConstant.SKILL_MIN_PAGE_SIZE,
                filter: (x =>
                    x.IsDeleted == skillFilterModel.IsDeleted &&
                    (string.IsNullOrEmpty(skillFilterModel.Search) ||
                     x.Type.ToLower().Contains(skillFilterModel.Search.ToLower()) ||
                     x.Name.ToLower().Contains(skillFilterModel.Search.ToLower()))),
                orderBy: (x =>
                {
                    switch (skillFilterModel.Order.ToLower())
                    {
                        case "name":
                            return skillFilterModel.OrderByDescending
                                ? x.OrderByDescending(x => x.Name)
                                : x.OrderBy(x => x.Name);
                        case "type":
                            return skillFilterModel.OrderByDescending
                                ? x.OrderByDescending(x => x.Type)
                                : x.OrderBy(x => x.Type);
                        default:
                            return skillFilterModel.OrderByDescending
                                ? x.OrderByDescending(x => x.CreationDate)
                                : x.OrderBy(x => x.CreationDate);
                    }
                })
            );

            if (skillList != null)
            {
                var skillModelList = _mapper.Map<List<SkillGroupModel>>(skillList.Data);
                return new Pagination<SkillGroupModel>(skillModelList, skillList.TotalCount, skillFilterModel.PageIndex,
                    skillFilterModel.PageSize);
            }
            
            return null;
        }

        public async Task<ResponseModel> UpdateSkill(Guid id, SkillUpdateModel skillUpdateModel)
        {
            var skill = await _unitOfWork.SkillRepository.GetSkillById(id);
            if(skill != null) {
                if (!string.IsNullOrEmpty(skillUpdateModel.Name))
                {
                    skill.Name = skillUpdateModel.Name;
                }
                if (!string.IsNullOrEmpty(skillUpdateModel.Description))
                {
                    skill.Description = skillUpdateModel.Description;
                }
                if (!string.IsNullOrEmpty(skillUpdateModel.Type))
                {
                    var category = await _unitOfWork.ProjectCategoryReposioty.GetByNames([skillUpdateModel.Type]);
                    if (category.Count == 0)
                    {
                        return new ResponseModel()
                        {
                            Status = false,
                            Message = "Type not found"
                        };
                    }
                    skill.Type = skillUpdateModel.Type;
                }
                
                
                _unitOfWork.SkillRepository.Update(skill);
                await _unitOfWork.SaveChangeAsync();
                return new ResponseModel()
                {
                    Status = true,
                    Message = "Skill updated successfully"
                };
            }
            return new ResponseModel()
            {
                Status = false,
                Message = "Skill not found"
            };
        }
    }
}
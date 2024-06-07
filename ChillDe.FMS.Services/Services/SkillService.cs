using AutoMapper;
using ChillDe.FMS.Repositories.Common;
using ChillDe.FMS.Repositories.Interfaces;
using ChillDe.FMS.Repositories.Models.SkillModels;
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

        public async Task<Pagination<SkillGroupModel>> GetAllSkillsGroupByType(SkillFilterModel skillFilterModel)
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
                            return x.OrderBy(x => x.CreationDate);
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
    }
}
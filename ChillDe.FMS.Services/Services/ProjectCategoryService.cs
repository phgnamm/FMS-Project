using AutoMapper;
using ChillDe.FMS.Repositories.Common;
using ChillDe.FMS.Repositories.Entities;
using ChillDe.FMS.Repositories.Interfaces;
using ChillDe.FMS.Repositories.Models.AccountModels;
using ChillDe.FMS.Repositories.ViewModels.AccountModels;
using ChillDe.FMS.Repositories.ViewModels.ResponseModels;
using ChillDe.FMS.Services.Models.ProjectCategoryModels;
using Services.Interfaces;
using System.Threading.Tasks;

namespace Services.Services
{
    public class ProjectCategoryService : IProjectCategoryService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IClaimsService _claimsService;

        public ProjectCategoryService(IUnitOfWork unitOfWork, IMapper mapper, IClaimsService claimsService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _claimsService = claimsService;
        }

        

        public async Task<Pagination<ProjectCategory>> GetProjectCategoriesByFilterAsync(ProjectCategoryFilterModel filterModel)
        {
            var accountList = await _unitOfWork.ProjectCategoryReposioty.GetProCateByFilter(pageIndex: filterModel.PageIndex,
            pageSize: filterModel.PageSize,
            filter: (x =>
            x.IsDeleted == filterModel.IsDeleted &&
            (filterModel.Name == null || x.Name == filterModel.Name) &&
                   (string.IsNullOrEmpty(filterModel.Search) ||
                    x.Name.ToLower().Contains(filterModel.Search.ToLower()) ||
                    x.Description.ToLower().Contains(filterModel.Search.ToLower()))),
               orderBy: (x =>
               {
                   switch (filterModel.Order.ToLower())
                   {
                       case "name":
                           return filterModel.OrderByDescending
                               ? x.OrderByDescending(x => x.Name)
                               : x.OrderBy(x => x.Name);
                       case "description":
                           return filterModel.OrderByDescending
                               ? x.OrderByDescending(x => x.Description)
                               : x.OrderBy(x => x.Description);

                       default:
                           return filterModel.OrderByDescending
                               ? x.OrderByDescending(x => x.CreationDate)
                               : x.OrderBy(x => x.CreationDate);
                   }
               })
           );

            if (accountList != null)
            {
                var accountModelList = _mapper.Map<List<ProjectCategory>>(accountList.Data);
                return new Pagination<ProjectCategory>(accountModelList, accountList.TotalCount, filterModel.PageIndex,
                    filterModel.PageSize);
            }

            return null; ;
        }

        public async Task<ResponseDataModel<List<ProjectCategoryModel>>> GetProjectCategoriesByNames(List<string> names)
        {
            var projectCategories = await _unitOfWork.ProjectCategoryReposioty.GetByNames(names);

            if (projectCategories == null || projectCategories.Count == 0)
            {
                return new ResponseDataModel<List<ProjectCategoryModel>>()
                {
                    Status = false,
                    Message = "No project categories found"
                };
            }

            var projectCategoryModels = _mapper.Map<List<ProjectCategoryModel>>(projectCategories);
            return new ResponseDataModel<List<ProjectCategoryModel>>()
            {
                Status = true,
                Message = "Get project categories successfully",
                Data = projectCategoryModels
            };
        }

        public async Task<ResponseDataModel<ProjectCategory>> UpdateProjectCategoryAsync(Guid id, ProjectCategoryUpdateModel updateModel)
        {
            var proCate = await _unitOfWork.ProjectCategoryReposioty.GetProjectCategoryById(id);
            if (proCate != null)
            {
                //var result = _mapper.Map(updateModel, proCate);
                if (!string.IsNullOrEmpty(updateModel.Name))
                {
                    proCate.Name = updateModel.Name;
                }
                if (updateModel.Description != null)
                {
                    proCate.Description = updateModel.Description;
                }
                _unitOfWork.ProjectCategoryReposioty.Update(proCate);
                await _unitOfWork.SaveChangeAsync();
                return new ResponseDataModel<ProjectCategory>()
                {
                    Status = true,
                    Message = "Update project category successfully",
                    Data = proCate
                };
            }
            return new ResponseDataModel<ProjectCategory>()
            {
                Status = false,
                Message = "Can not find project category. Update fail"
            };
        }
        public async Task<ResponseModel> CreateProjectCategoyryAsync(List<ProjectCategoryCreateModel> createModel)
        {
            var proCate = _mapper.Map<List<ProjectCategory>>(createModel);
            if (proCate != null)
            {
                foreach (var item in proCate)
                {  
                    await _unitOfWork.ProjectCategoryReposioty.AddAsync(item);
                }
                await _unitOfWork.SaveChangeAsync();
                return new ResponseModel()
                {
                    Status = true,
                    Message = "Create project category successfully",
                    
                };
            }
            return new ResponseModel()
            {
                Status = false,
                Message = "Create project category fail"
            };
        }
        public async Task<ResponseModel> BlockProjectCategoryAsync(Guid id)
        {
            var proCate = await _unitOfWork.ProjectCategoryReposioty.GetProjectCategoryById(id);
            
            if (proCate != null)
            {
                var skillListwithProCate = await _unitOfWork.SkillRepository.GetAllByType(proCate.Name);
                proCate.IsDeleted = true;
                _unitOfWork.ProjectCategoryReposioty.SoftDelete(proCate);
                if(skillListwithProCate != null)
                {
                    foreach (var item in skillListwithProCate)
                    {
                        item.IsDeleted = true;
                        _unitOfWork.SkillRepository.SoftDelete(item);
                    }
                }
                await _unitOfWork.SaveChangeAsync();
                return new ResponseModel()
                {
                    Status = true,
                    Message = "Block project category successfully",
                };
            }
            return new ResponseModel()
            {
                Status = false,
                Message = "Can not find project category. Block fail"
            };
        }
    }
}

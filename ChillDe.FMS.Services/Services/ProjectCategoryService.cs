using AutoMapper;
using ChillDe.FMS.Repositories.Entities;
using ChillDe.FMS.Repositories.Interfaces;
using ChillDe.FMS.Repositories.ViewModels.ResponseModels;
using ChillDe.FMS.Services.Models.ProjectCategoryModels;
using Services.Interfaces;

namespace Services.Services
{
    public class ProjectCategoryService : IProjectCategoryService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ProjectCategoryService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
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
    }
}

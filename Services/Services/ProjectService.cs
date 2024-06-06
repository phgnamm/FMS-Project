using AutoMapper;
using Repositories.Common;
using Repositories.Entities;
using Repositories.Enums;
using Repositories.Interfaces;
using Repositories.ViewModels.FreelancerModels;
using Repositories.ViewModels.ProjectModels;
using Repositories.ViewModels.ResponseModels;
using Services.Interfaces;

namespace Services.Services
{
    public class ProjectService : IProjectService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ProjectService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ResponseDataModel<ProjectAddModel>> CreateProject(ProjectAddModel projectModel)
        {
            var existingProject = await _unitOfWork.ProjectRepository.GetProjectByCode(projectModel.Code);
            if (existingProject != null)
            {
                return new ResponseDataModel<ProjectAddModel>()
                {
                    Message = "Project's code already existed!",
                    Status = false
                };
            }
            
            var account = await _unitOfWork.AccountRepository.GetAccountByCode(projectModel.AccountCode);
            if (account == null)
            {
                return new ResponseDataModel<ProjectAddModel>()
                {
                    Message = "User not found",
                    Status = false
                };
            }

            var category = await _unitOfWork.ProjectCategoryReposioty.GetAsync(projectModel.CategoryId);
            if (category == null)
            {
                return new ResponseDataModel<ProjectAddModel>()
                {
                    Message = "Category not found",
                    Status = false
                };
            }

            Project project = _mapper.Map<Project>(projectModel);
            project.AccountId = account.Id;
            project.CategoryId = category.Id;
            project.Status = ProjectStatus.Pending.ToString();
            await _unitOfWork.ProjectRepository.AddAsync(project);
            await _unitOfWork.SaveChangeAsync();
            return new ResponseDataModel<ProjectAddModel>()
            {
                Message = "Create project successfully!",
                Status = true
            };
        }

        public async Task<Pagination<ProjectDetailModel>> GetProjectsByFilter
            (PaginationParameter paginationParameter, ProjectDetailModel projectDetailModel)
        {
            //var projects = await _unitOfWork.FreelancerRepository.GetFreelancersByFilter(paginationParameter, freelancerFilterModel);

            //// Pagination
            //if (freelancerList != null)
            //{
            //    var totalCount = freelancerList.Count();
            //    // var freelancerListModel = _mapper.Map<List<FreelancerModel>>(freelancerList);

            //    var paginationList = freelancerList
            //        .Skip((paginationParameter.PageIndex - 1) * paginationParameter.PageSize)
            //        .Take(paginationParameter.PageSize)
            //        .ToList();

            //    return new Pagination<FreelancerDetailModel>(paginationList, totalCount, paginationParameter.PageIndex,
            //        paginationParameter.PageSize);
            //}

            return null;
        }
    }
}

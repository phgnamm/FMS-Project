using AutoMapper;
using ChillDe.FMS.Repositories.Common;
using ChillDe.FMS.Repositories.Entities;
using ChillDe.FMS.Repositories.Enums;
using ChillDe.FMS.Repositories.Interfaces;
using ChillDe.FMS.Repositories.ViewModels.ResponseModels;
using ChillDe.FMS.Services.Models.ProjectModels;
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

        public async Task<ResponseDataModel<ProjectModel>> CreateProject(ProjectCreateModel projectModel)
        {
            var existingProject = await _unitOfWork.ProjectRepository.GetProjectByCode(projectModel.Code);
            if (existingProject != null)
            {
                return new ResponseDataModel<ProjectModel>()
                {
                    Message = "Project's code already existed!",
                    Status = false
                };
            }

            var account = await _unitOfWork.AccountRepository.GetAccountById(projectModel.AccountId);
            if (account == null)
            {
                return new ResponseDataModel<ProjectModel>()
                {
                    Message = "User not found",
                    Status = false
                };
            }

            var category = await _unitOfWork.ProjectCategoryReposioty.GetAsync(projectModel.ProjectCategoryId);
            if (category == null)
            {
                return new ResponseDataModel<ProjectModel>()
                {
                    Message = "Category not found",
                    Status = false
                };
            }

            Project project = _mapper.Map<Project>(projectModel);

            if (project.Deposit >= project.Price)
            {
                return new ResponseDataModel<ProjectModel>()
                {
                    Message = "Deposit must be less than price",
                    Status = false
                };
            }

            project.AccountId = account.Id;
            project.ProjectCategoryId = category.Id;
            project.Status = ProjectStatus.Pending;
            project.Visibility = projectModel.Visibility;
            await _unitOfWork.ProjectRepository.AddAsync(project);

            //Create project deliverable
            //if(projectModel.ProjectDeliverableCreateModel != null)
            //{
            //    var projectDeliverable = _mapper.Map<ProjectDeliverable>
            //        (projectModel.ProjectDeliverableCreateModel);
            //    var deliverableType = await _unitOfWork.DeliverableTypeRepository
            //        .GetAsync((Guid)projectDeliverable.DeliverableTypeId);
            //    if (deliverableType == null)
            //    {
            //        return new ResponseDataModel<ProjectCreateModel>()
            //        {
            //            Message = "Deliverable type not found.",
            //            Status = false
            //        };
            //    }
            //    projectDeliverable.ProjectId = project.Id;
            //    projectDeliverable.Status = ProjectDeliverableStatus.Checking;
            //    await _unitOfWork.ProjectDeliverableRepository.AddAsync(projectDeliverable);
            //}

            //Create project apply
            if (projectModel.FreelancerId != null)
            {
                var freelancer = await _unitOfWork.FreelancerRepository
                    .GetAsync((Guid)projectModel.FreelancerId);
                if (freelancer == null)
                {
                    return new ResponseDataModel<ProjectModel>()
                    {
                        Message = "Freelancer not found.",
                        Status = false
                    };
                }
                ProjectApply projectApply = new()
                {
                    FreelancerId = projectModel.FreelancerId,
                };
                projectApply.ProjectId = project.Id;
                projectApply.Status = ProjectApplyStatus.Accepted;
                projectApply.StartDate = DateTime.UtcNow;
                projectApply.EndDate = DateTime.UtcNow.AddDays(project.Duration);
                await _unitOfWork.ProjectApplyRepository.AddAsync(projectApply);
            }

            await _unitOfWork.SaveChangeAsync();

            if (projectModel.FreelancerId != null)
            {
                project.Status = ProjectStatus.Processing;
            }
            else
            {
                project.Status = ProjectStatus.Pending;
            }
            _unitOfWork.ProjectRepository.Update(project);
            await _unitOfWork.SaveChangeAsync();

            ProjectCreateModel projectCreateModel = _mapper.Map<ProjectCreateModel>(project);
            projectCreateModel.FreelancerId = projectModel.FreelancerId;

            var result = _mapper.Map<ProjectModel>(project);

            return new ResponseDataModel<ProjectModel>()
            {
                Message = "Create project successfully!",
                Status = true,
                Data = result
            };
        }

        public async Task<ResponseDataModel<ProjectModel>> GetProject(Guid id)
        {
            var project = await _unitOfWork.ProjectRepository.GetAsync(id);

            if (project == null)
            {
                return new ResponseDataModel<ProjectModel>()
                {
                    Status = false,
                    Message = "Project not found"
                };
            }

            var projectModel = _mapper.Map<ProjectModel>(project);

            var account = await _unitOfWork.AccountRepository.GetAccountById((Guid)project.AccountId);
            if (account != null)
            {
                projectModel.AccountEmail = account.Email;
                projectModel.AccountFirstName = account.FirstName;
                projectModel.AccountLastName = account.LastName;
            }

            var category = await _unitOfWork.ProjectCategoryReposioty.GetAsync((Guid)project.ProjectCategoryId);
            if (category != null)
            {
                projectModel.ProjectCategoryName = category.Name;
            }

            return new ResponseDataModel<ProjectModel>()
            {
                Status = true,
                Message = "Get project successfully",
                Data = projectModel
            };
        }

        public async Task<Pagination<ProjectModel>> GetAllProjects(ProjectFilterModel projectFilterModel)
        {
            var projectList = await _unitOfWork.ProjectRepository.GetAllAsync(
            filter: x =>
                x.IsDeleted == projectFilterModel.IsDeleted &&
                (projectFilterModel.Status == null || x.Status == projectFilterModel.Status) &&
                (projectFilterModel.ProjectCategoryId == null || projectFilterModel.ProjectCategoryId.Count == 0 || projectFilterModel.ProjectCategoryId.Contains((Guid)x.ProjectCategoryId)) &&
                (projectFilterModel.Visibility == null || x.Visibility == projectFilterModel.Visibility) &&
                (projectFilterModel.AccountId == null || x.AccountId == projectFilterModel.AccountId) &&
                (string.IsNullOrEmpty(projectFilterModel.Search) ||
                 x.Name.ToLower().Contains(projectFilterModel.Search.ToLower()) ||
                 x.Code.ToLower().Contains(projectFilterModel.Search.ToLower())),

            orderBy: x =>
            {
                switch (projectFilterModel.Order.ToLower())
                {
                    case "name":
                        return projectFilterModel.OrderByDescending
                            ? x.OrderByDescending(x => x.Name)
                            : x.OrderBy(x => x.Name);
                    case "price":
                        return projectFilterModel.OrderByDescending
                            ? x.OrderByDescending(x => x.Price)
                            : x.OrderBy(x => x.Price);
                    case "code":
                        return projectFilterModel.OrderByDescending
                            ? x.OrderByDescending(x => x.Code)
                            : x.OrderBy(x => x.Code);
                    default:
                        return projectFilterModel.OrderByDescending
                            ? x.OrderByDescending(x => x.CreationDate)
                            : x.OrderBy(x => x.CreationDate);
                }
            },
            pageIndex: projectFilterModel.PageIndex,
            pageSize: projectFilterModel.PageSize,
            includeProperties: "Account,ProjectCategory"

        );

            if (projectList != null)
            {
                var projectDetailList = projectList.Data.Select(p => new ProjectModel
                {
                    Id = p.Id,
                    Name = p.Name,
                    Code = p.Code,
                    Description = p.Description,
                    Duration = p.Duration,
                    Price = p.Price,
                    Deposit = p.Deposit,
                    Status = p.Status,
                    Visibility = p.Visibility,
                    AccountId = p.AccountId,
                    AccountEmail = p.Account.Email,
                    AccountFirstName = p.Account.FirstName,
                    AccountLastName = p.Account.LastName,
                    ProjectCategoryId = p.ProjectCategoryId,
                    ProjectCategoryName = p.ProjectCategory.Name
                }).ToList();

                return new Pagination<ProjectModel>(projectDetailList, projectList.TotalCount, projectFilterModel.PageIndex, projectFilterModel.PageSize);
            }
            return null;
        }

        public async Task<ResponseDataModel<ProjectModel>> UpdateProject(Guid id, ProjectUpdateModel updateProject)
        {
            var existingProject = await _unitOfWork.ProjectRepository.GetAsync(id);
            if (existingProject != null)
            {
                var existingCode = await _unitOfWork.ProjectRepository.GetProjectByCode(updateProject.Code);
                if (existingCode != null && existingCode.Code != updateProject.Code)
                {
                    return new ResponseDataModel<ProjectModel>()
                    {
                        Message = "Project's code already existed!",
                        Status = false
                    };
                }

                var projectCategory = await _unitOfWork.ProjectCategoryReposioty
                    .GetAsync(updateProject.ProjectCategoryId);
                if (projectCategory == null)
                {
                    return new ResponseDataModel<ProjectModel>()
                    {
                        Status = false,
                        Message = "Category not found"
                    };
                }

                if (updateProject.Deposit >= updateProject.Price)
                {
                    return new ResponseDataModel<ProjectModel>()
                    {
                        Message = "Deposit must be less than price",
                        Status = false
                    };
                }

                if (existingProject.Duration != updateProject.Duration)
                {
                    var projectApply = await _unitOfWork.ProjectApplyRepository
                        .GetAcceptedProjectApplyByProjectId(id);
                    if (projectApply.StartDate.Value.AddDays(updateProject.Duration) <=
                        DateTime.UtcNow)
                    {
                        return new ResponseDataModel<ProjectModel>()
                        {
                            Status = false,
                            Message = "Duration is out of time."
                        };
                    }
                    projectApply.EndDate = projectApply.StartDate.Value.AddDays(updateProject.Duration);
                    _unitOfWork.ProjectApplyRepository.Update(projectApply);
                }

                existingProject = _mapper.Map(updateProject, existingProject);

                _unitOfWork.ProjectRepository.Update(existingProject);
                await _unitOfWork.SaveChangeAsync();
                var result = _mapper.Map<ProjectModel>(existingProject);
                if (result != null)
                {
                    return new ResponseDataModel<ProjectModel>()
                    {
                        Status = true,
                        Message = "Update project successfully",
                        Data = result
                    };
                }
                return new ResponseDataModel<ProjectModel>()
                {
                    Status = false,
                    Message = "Update project fail"
                };
            }
            return new ResponseDataModel<ProjectModel>()
            {
                Status = false,
                Message = "Project not found"
            };
        }

        public async Task<ResponseDataModel<ProjectModel>> DeleteProject(Guid id)
        {
            var project = await _unitOfWork.ProjectRepository.GetAsync(id);
            if (project != null)
            {
                var result = _mapper.Map<ProjectModel>(project);
                _unitOfWork.ProjectRepository.SoftDelete(project);
                await _unitOfWork.SaveChangeAsync();
                if (result != null)
                {
                    return new ResponseDataModel<ProjectModel>()
                    {
                        Status = true,
                        Message = "Delete project successfully",
                        Data = result
                    };
                }
                return new ResponseDataModel<ProjectModel>()
                {
                    Status = false,
                    Message = "Delete project failed"
                };
            }
            return new ResponseDataModel<ProjectModel>()
            {
                Status = false,
                Message = "Project not found"
            };
        }

        public async Task<ResponseDataModel<ProjectModel>> CloseProject(Guid projectId, ProjectStatus status)
        {
            var project = await _unitOfWork.ProjectRepository.GetAsync(projectId);
            if (project != null)
            {
                var projectApply = await _unitOfWork.ProjectApplyRepository
                    .GetAcceptedProjectApplyByProjectId(projectId);
                if (projectApply != null)
                {
                    DateTime startDate = (DateTime)projectApply.StartDate;
                    projectApply.EndDate = startDate.Add(TimeSpan.FromDays(project.Duration));
                }

                var freelancer = await _unitOfWork.FreelancerRepository.GetFreelancerByProjectId(projectId);
                if (status == ProjectStatus.Closed)
                {
                    if (freelancer != null)
                    {
                        var deliverableProduct = await _unitOfWork.DeliverableProductRepository
                            .GetByProjectApplyId(projectApply.Id);
                        if (deliverableProduct != null)
                        {
                            var projectDeliverable = await _unitOfWork.ProjectDeliverableRepository
                            .GetAsync((Guid)deliverableProduct.ProjectDeliverableId);
                            if (projectDeliverable != null)
                                freelancer.Wallet += project.Deposit;
                        }
                    }
                }

                if (status == ProjectStatus.Done)
                {
                    if (freelancer != null)
                    {
                        freelancer.Wallet += (float)project.Price;
                    }
                }

                project.Status = status;
                var result = _mapper.Map<ProjectModel>(project);
                _unitOfWork.ProjectRepository.Update(project);
                _unitOfWork.FreelancerRepository.Update(freelancer);
                await _unitOfWork.SaveChangeAsync();
                if (result != null)
                {
                    return new ResponseDataModel<ProjectModel>()
                    {
                        Status = true,
                        Message = "Delete project successfully",
                        Data = result
                    };
                }
                return new ResponseDataModel<ProjectModel>()
                {
                    Status = false,
                    Message = "Delete project failed"
                };
            }
            return new ResponseDataModel<ProjectModel>()
            {
                Status = false,
                Message = "Project not found"
            };
        }
    }
}

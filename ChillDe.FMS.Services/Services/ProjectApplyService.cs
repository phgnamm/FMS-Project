using AutoMapper;
using ChillDe.FMS.Repositories.Common;
using ChillDe.FMS.Repositories.Entities;
using ChillDe.FMS.Repositories.Enums;
using ChillDe.FMS.Repositories.Interfaces;
using ChillDe.FMS.Repositories.Models.SkillModels;
using ChillDe.FMS.Repositories.ViewModels.ResponseModels;
using ChillDe.FMS.Services.Common;
using ChillDe.FMS.Services.Interfaces;
using ChillDe.FMS.Services.Models.ProjectApplyModels;
using ChillDe.FMS.Services.Models.ProjectModels;
using Quartz;

namespace ChillDe.FMS.Services.Services
{
    public class ProjectApplyService : IProjectApplyService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IScheduler _scheduler;

        public ProjectApplyService(IUnitOfWork unitOfWork, IMapper mapper, IScheduler scheduler)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _scheduler = scheduler;
        }
        public async Task<ResponseModel> AddProjectApply(ProjectApplyCreateModel projectApplyModel)
        {
            var project = await _unitOfWork.ProjectRepository.GetAsync(projectApplyModel.ProjectId);
            if (project == null)
            {
                return new ResponseModel
                {
                    Message = "Project not found",
                    Status = false
                };
            }

            var freelancer = await _unitOfWork.FreelancerRepository.GetAsync(projectApplyModel.FreelancerId);
            if (freelancer == null)
            {
                return new ResponseModel
                {
                    Message = "Freelancer not found",
                    Status = false
                };
            }

            var projectApply = new ProjectApply();
            projectApply.ProjectId = project.Id;
            projectApply.FreelancerId = freelancer.Id;
            if (project.Visibility == ProjectVisibility.Public)
            {
                projectApply.Status = ProjectApplyStatus.Checking;
            }
            else if (project.Visibility == ProjectVisibility.Private)
            {
                projectApply.Status = ProjectApplyStatus.Invited;
            }

            await _unitOfWork.ProjectApplyRepository.AddAsync(projectApply);
            _unitOfWork.ProjectRepository.Update(project);
            await _unitOfWork.SaveChangeAsync();

            return new ResponseModel
            {
                Message = "Apply successfully",
                Status = true
            };
        }

        public async Task<ResponseModel> UpdateProjectApply(ProjectApplyUpdateModel projectApplyUpdateModel)
        {
            var existingProjectApply = await _unitOfWork.ProjectApplyRepository
        .GetAsync(projectApplyUpdateModel.Id, includeProperties: "Project");
            if (existingProjectApply == null)
            {
                return new ResponseModel
                {
                    Message = "Project apply not found",
                    Status = false
                };
            }

            if (projectApplyUpdateModel.Status == ProjectApplyStatus.Accepted)
            {
                existingProjectApply.StartDate = DateTime.UtcNow;
                existingProjectApply.EndDate = DateTime.UtcNow.AddDays(existingProjectApply.Project.Duration);
                existingProjectApply.Project.Status = ProjectStatus.Processing;
            }

            if (projectApplyUpdateModel.Status != null)
            {
                existingProjectApply.Status = projectApplyUpdateModel.Status;
            }

            if (projectApplyUpdateModel.EndDate != null && existingProjectApply is { Status: ProjectApplyStatus.Accepted, StartDate: not null, EndDate: not null })
            {
                if (existingProjectApply.StartDate <= DateTime.UtcNow &&
                    projectApplyUpdateModel.EndDate <= DateTime.UtcNow)
                {
                    return new ResponseModel
                    {
                        Message = "End date must be later than today",
                        Status = false
                    };
                }
                else if (existingProjectApply.StartDate > DateTime.UtcNow &&
                           projectApplyUpdateModel.EndDate <= existingProjectApply.StartDate)
                {
                    return new ResponseModel
                    {
                        Message = "End date must be later than start date",
                        Status = false
                    };
                }

                existingProjectApply.EndDate = projectApplyUpdateModel.EndDate;
                TimeSpan? newDuration = existingProjectApply.EndDate - existingProjectApply.StartDate;
                existingProjectApply.Project.Duration = newDuration.Value.Days;
            }

            _unitOfWork.ProjectRepository.Update(existingProjectApply.Project);
            _unitOfWork.ProjectApplyRepository.Update(existingProjectApply);
            await _unitOfWork.SaveChangeAsync();

            // Schedule the warning email job if project apply is accepted
            if (projectApplyUpdateModel.Status == ProjectApplyStatus.Accepted)
            {
                var jobData = new JobDataMap();
                jobData.Put("projectApplyId", existingProjectApply.Id);

                IJobDetail job = JobBuilder.Create<WarningEmailJob>()
                    .UsingJobData(jobData)
                    .Build();

                ITrigger trigger = TriggerBuilder.Create()
                    .StartAt(existingProjectApply.EndDate.Value.AddDays(-1)) // Send email one day before the end date
                    .Build();

                await _scheduler.ScheduleJob(job, trigger);
            }

            return new ResponseModel
            {
                Message = "Update successfully",
                Status = true
            };
        }

        public async Task<ResponseModel> DeleteProjectApply(Guid projectApplyId)
        {
            var existingProjectApply = await _unitOfWork.ProjectApplyRepository.GetAsync(projectApplyId);
            if (existingProjectApply == null)
            {
                return new ResponseModel
                {
                    Message = "ProjectApply not found",
                    Status = false
                };
            }
            if (existingProjectApply.Status == ProjectApplyStatus.Invited || existingProjectApply.Status == ProjectApplyStatus.Checking)
            {
                _unitOfWork.ProjectApplyRepository.HardDelete(existingProjectApply);
            }
            else if (existingProjectApply.Status == ProjectApplyStatus.Accepted)
            {
                if (existingProjectApply.EndDate < DateTime.UtcNow)
                {
                    _unitOfWork.ProjectApplyRepository.SoftDelete(existingProjectApply);
                }
                else
                {
                    return new ResponseModel
                    {
                        Message = "This Apply can be Delete when accepted",
                        Status = false
                    };
                }
            }
            await _unitOfWork.SaveChangeAsync();
            return new ResponseModel
            {
                Message = "Delete successfully",
                Status = true
            };
        }

        public async Task<Pagination<ProjectApplyModel>> GetProjectAppliesByFilter(ProjectApplyFilterModel projectApplyFilterModel)
        {
            var projectApplyList = await _unitOfWork.ProjectApplyRepository.GetAllAsync(
                filter: x =>
                    x.IsDeleted == projectApplyFilterModel.IsDeleted &&
                    (projectApplyFilterModel.ProjectId == null || x.Project.Id == projectApplyFilterModel.ProjectId) &&
                    (projectApplyFilterModel.FreelancerId == null || x.Freelancer.Id == projectApplyFilterModel.FreelancerId) &&
                    (projectApplyFilterModel.Gender == null || x.Freelancer.Gender == projectApplyFilterModel.Gender) &&
                    (projectApplyFilterModel.Status == null || x.Status == projectApplyFilterModel.Status) &&
                    (projectApplyFilterModel.SkillType == null || x.Freelancer.FreelancerSkills.Any(fs => fs.Skill.Type == projectApplyFilterModel.SkillType)) &&
                    (projectApplyFilterModel.SkillName == null || x.Freelancer.FreelancerSkills.Any(fs => fs.Skill.Name.Contains(projectApplyFilterModel.SkillName))) &&
                    (string.IsNullOrEmpty(projectApplyFilterModel.Search) ||
                     x.Freelancer.FirstName.ToLower().Contains(projectApplyFilterModel.Search.ToLower()) ||
                     x.Freelancer.LastName.ToLower().Contains(projectApplyFilterModel.Search.ToLower()) ||
                     x.Freelancer.Code.ToLower().Contains(projectApplyFilterModel.Search.ToLower()) ||
                     x.Freelancer.Email.ToLower().Contains(projectApplyFilterModel.Search.ToLower())),
                orderBy: query =>
                {
                    switch (projectApplyFilterModel.Order.ToLower())
                    {
                        case "first-name":
                            return projectApplyFilterModel.OrderByDescending
                                ? query.OrderByDescending(x => x.Freelancer.FirstName)
                                : query.OrderBy(x => x.Freelancer.FirstName);
                        case "last-name":
                            return projectApplyFilterModel.OrderByDescending
                                ? query.OrderByDescending(x => x.Freelancer.LastName)
                                : query.OrderBy(x => x.Freelancer.LastName);
                        case "code":
                            return projectApplyFilterModel.OrderByDescending
                                ? query.OrderByDescending(x => x.Freelancer.Code)
                                : query.OrderBy(x => x.Freelancer.Code);
                        case "date-of-birth":
                            return projectApplyFilterModel.OrderByDescending
                                ? query.OrderByDescending(x => x.Freelancer.DateOfBirth)
                                : query.OrderBy(x => x.Freelancer.DateOfBirth);
                        default:
                            return projectApplyFilterModel.OrderByDescending
                                ? query.OrderByDescending(x => x.CreationDate)
                                : query.OrderBy(x => x.CreationDate); ;
                    }
                },
                pageIndex: projectApplyFilterModel.PageIndex,
                pageSize: projectApplyFilterModel.PageSize,
                includeProperties: "Freelancer.FreelancerSkills.Skill, Project, Project.Account, Project.ProjectCategory"
            );

            if (projectApplyList != null)
            {
                var projectApplyDetailList = projectApplyList.Data.Select(f => new ProjectApplyModel
                {
                    Id = f.Id,
                    ProjectId = f.Project.Id,
                    Image = f.Freelancer.Image,
                    Status = f.Status,
                    CreationDate = f.CreationDate,
                    FreelancerId = f.Freelancer.Id,
                    FreelancerFirstName = f.Freelancer.FirstName,
                    FreelancerLastName = f.Freelancer.LastName,
                    Project = _mapper.Map<ProjectModel>(f.Project),
                    StartDate = f.StartDate,
                    EndDate = f.EndDate,
                    Skills = f.Freelancer.FreelancerSkills.GroupBy(fs => fs.Skill.Type)
                            .Select(group => new SkillGroupModel
                            {
                                SkillType = group.Key,
                                SkillNames = group.Select(fs => fs.Skill.Name).ToList()
                            }).ToList()
                }).ToList();

                return new Pagination<ProjectApplyModel>(projectApplyDetailList, projectApplyList.TotalCount, projectApplyFilterModel.PageIndex, projectApplyFilterModel.PageSize);
            }

            return null;
        }

    }
}

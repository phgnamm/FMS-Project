﻿using AutoMapper;
using ChillDe.FMS.Repositories.Common;
using ChillDe.FMS.Repositories.Entities;
using ChillDe.FMS.Repositories.Enums;
using ChillDe.FMS.Repositories.Interfaces;
using ChillDe.FMS.Repositories.ViewModels.FreelancerModels;
using ChillDe.FMS.Repositories.ViewModels.ResponseModels;
using ChillDe.FMS.Services.Interfaces;
using ChillDe.FMS.Services.Models.ProjectApplyModels;
using ChillDe.FMS.Services.ViewModels.FreelancerModels;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChillDe.FMS.Services.Services
{
    public class ProjectApplyService : IProjectApplyService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ProjectApplyService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ResponseDataModel<FreelancerDetailModel>> AddRangeProjectApply
            (List<Guid> freelancerIds, Guid projectId)
        {
            var project = await _unitOfWork.ProjectRepository.GetAsync(projectId);
            if (project == null)
            {
                return new ResponseDataModel<FreelancerDetailModel>
                {
                    Message = "Project not found",
                    Status = false
                };
            }

            List<ProjectApply> projectApplies = new List<ProjectApply>();
            foreach (var freelancerId in freelancerIds)
            {
                var freelancer = _unitOfWork.FreelancerRepository.GetFreelancerById(freelancerId);
                if (freelancer == null)
                {
                    return new ResponseDataModel<FreelancerDetailModel>
                    {
                        Message = "Freelancer not found",
                        Data = _mapper.Map<FreelancerDetailModel>(freelancer),
                        Status = false
                    };
                }
                else
                {
                    var projectApply = new ProjectApply();
                    projectApply.FreelancerId = freelancerId;
                    projectApply.ProjectId = projectId;
                    projectApply.StartDate = DateTime.UtcNow;
                    projectApply.Status = ProjectApplyStatus.Accepted;
                    projectApplies.Add(projectApply);
                }
            }

            await _unitOfWork.ProjectApplyRepository.AddRangeAsync(projectApplies);
            await _unitOfWork.SaveChangeAsync();

            return new ResponseDataModel<FreelancerDetailModel>
            {
                Message = "Add succesfully",
                Status = true
            };
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
            else
            {
                projectApply.Status = ProjectApplyStatus.Invited;
            }

            await _unitOfWork.ProjectApplyRepository.AddAsync(projectApply);
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
                .GetAsync(projectApplyUpdateModel.Id);
            if (existingProjectApply == null)
            {
                return new ResponseModel
                {
                    Message = "Project apply not found",
                    Status = false
                };
            }

            if (projectApplyUpdateModel.Status == 2)
            {
                existingProjectApply.StartDate = DateTime.UtcNow;
                existingProjectApply.Status = ProjectApplyStatus.Accepted;
            }
            if (projectApplyUpdateModel.Status == 3)
            {
                existingProjectApply.Status = ProjectApplyStatus.Rejected;
            }

            _unitOfWork.ProjectApplyRepository.Update(existingProjectApply);
            await _unitOfWork.SaveChangeAsync();

            return new ResponseModel
            {
                Message = "Update successfully",
                Status = true
            };
        }

        public async Task<Pagination<ProjectApplyModel>> GetProjectAppliesByFilter(ProjectApplyFilterModel projectApplyFilterModel)
        {
            var projectApplyList = await _unitOfWork.ProjectApplyRepository.GetAllAsync(
                filter: x => x.Project.Id == projectApplyFilterModel.ProjectId &&
                    (projectApplyFilterModel.Gender == null || x.Freelancer.Gender == projectApplyFilterModel.Gender) &&
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
                            return query.OrderBy(x => x.CreationDate);
                    }
                },
                pageIndex: projectApplyFilterModel.PageIndex,
                pageSize: projectApplyFilterModel.PageSize,
                includeProperties: "Freelancer.FreelancerSkills.Skill, Project"
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
                }).ToList();

                return new Pagination<ProjectApplyModel>(projectApplyDetailList, projectApplyList.TotalCount, projectApplyFilterModel.PageIndex, projectApplyFilterModel.PageSize);
            }

            return null;
        }

    }
}

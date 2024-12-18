using ChillDe.FMS.Repositories.Entities;
using ChillDe.FMS.Repositories.Enums;
using ChillDe.FMS.Repositories.Interfaces;
using ChillDe.FMS.Repositories.ViewModels.ResponseModels;
using ChillDe.FMS.Services.Interfaces;
using ChillDe.FMS.Services.Models.DashboardModels;
using ChillDe.FMS.Services.Models.DeliverableProductModels;
using ChillDe.FMS.Services.Models.ProjectApplyModels;
using ChillDe.FMS.Services.Models.ProjectModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Services.Interfaces;

namespace ChillDe.FMS.Services.Services;

public class DashboardService : IDashboardService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IClaimsService _claimsService;
    private readonly UserManager<Account> _userManager;
    private readonly IProjectService _projectService;
    private readonly IDeliverableProductService _deliverableProductService;
    private readonly IProjectApplyService _projectApplyService;

    public DashboardService(IUnitOfWork unitOfWork, IClaimsService claimsService, UserManager<Account> userManager,
        IProjectService projectService, IDeliverableProductService deliverableProductService,
        IProjectApplyService projectApplyService)
    {
        _unitOfWork = unitOfWork;
        _claimsService = claimsService;
        _userManager = userManager;
        _projectService = projectService;
        _deliverableProductService = deliverableProductService;
        _projectApplyService = projectApplyService;
    }

    public async Task<ResponseDataModel<AdministratorDashboardModel>> GetAdminDashboard()
    {
        var userId = _claimsService.GetCurrentUserId;
        var numOfFreelancer = await _unitOfWork.DbContext.Freelancer.CountAsync();
        var numOfAccount = await _userManager.Users.CountAsync();
        var totalPaid = await _unitOfWork.DbContext.Project.Where(x => x.Status == ProjectStatus.Done)
            .SumAsync(x => x.Price);
        var numberOfProject = await _unitOfWork.DbContext.Project.Where(x => x.IsDeleted == false).CountAsync();
        var commonDashboard = await GetCommonDashboard(userId.Value);

        return new ResponseDataModel<AdministratorDashboardModel>()
        {
            Status = true,
            Message = "Get admin dashboard successfully",
            Data = new AdministratorDashboardModel()
            {
                NumOfFreelancer = numOfFreelancer,
                NumOfAccount = numOfAccount,
                TotalPaid = (float)totalPaid,
                NumOfProject = numberOfProject,
                NumOfYourOngoingProject = commonDashboard.NumOfYourOngoingProject,
                NumOfWaitingChecking = commonDashboard.NumOfWaitingChecking,
                YourTotalPaid = commonDashboard.YourTotalPaid,
                RecentProducts = commonDashboard.RecentProducts,
                RecentProjects = commonDashboard.RecentProjects
            }
        };
    }

    public async Task<ResponseDataModel<StaffDashboardModel>> GetStaffDashboard()
    {
        var userId = _claimsService.GetCurrentUserId;
        var commonDashboard = await GetCommonDashboard(userId.Value);

        return new ResponseDataModel<StaffDashboardModel>()
        {
            Status = true,
            Message = "Get staff dashboard successfully",
            Data = commonDashboard
        };
    }

    public async Task<ResponseDataModel<FreelancerDashboardModel>> GetFreelancerDashboard()
    {
        var userId = _claimsService.GetCurrentUserId;
        var freelancer = await _unitOfWork.FreelancerRepository.GetFreelancerById(userId.Value);
        var numOfYourOngoingProject = await _unitOfWork.DbContext.ProjectApply.Where(x =>
            x.Status == ProjectApplyStatus.Accepted && (x.Project.Status == ProjectStatus.Processing ||
                                                        x.Project.Status == ProjectStatus.Checking) &&
            x.IsDeleted == false && x.Project.IsDeleted == false && x.FreelancerId == userId.Value).CountAsync();
        var remainTasks = await _unitOfWork.DbContext.ProjectDeliverable.Where(pd =>
                pd.Project.ProjectApplies.Any(pa =>
                    pa.FreelancerId == userId && pa.Status == ProjectApplyStatus.Accepted && pa.IsDeleted == false))
            .Where(x => x.Status != ProjectDeliverableStatus.Accepted && x.IsDeleted == false)
            .CountAsync();
        var recentProjects = await _projectApplyService.GetProjectAppliesByFilter(new ProjectApplyFilterModel()
        {
            FreelancerId = userId
        });
        var recentProducts = await _deliverableProductService.GetAllDeliverableProduct(
            new DeliverableProductFilterModel()
            {
                FreelancerId = userId
            });
        return new ResponseDataModel<FreelancerDashboardModel>()
        {
            Status = true,
            Message = "Get freelaner dashboard successfully",
            Data = new FreelancerDashboardModel()
            {
                RecentProjects = recentProjects,
                RecentProducts = recentProducts,
                Wallet = freelancer.Wallet,
                Warning = freelancer.Warning.Value,
                RemainTasks = remainTasks,
                NumOfYourOngoingProject = numOfYourOngoingProject
            }
        };
    }

    private async Task<StaffDashboardModel> GetCommonDashboard(Guid accountId)
    {
        var numOfYourOngoingProject = await _unitOfWork.DbContext.Project
            .Where(x => x.Status != ProjectStatus.Done && x.Status != ProjectStatus.Closed && x.AccountId == accountId && x.IsDeleted == false)
            .CountAsync();
        var numOfWaitingChecking = await _unitOfWork.DbContext.DeliverableProduct.Include(x => x.ProjectApply)
            .ThenInclude(x => x.Project).Where(x =>
                x.Status == DeliverableProductStatus.Checking && x.ProjectApply.Project.AccountId == accountId && x.IsDeleted == false)
            .CountAsync();
        var yourTotalPaid = await _unitOfWork.DbContext.Project
            .Where(x => x.AccountId == accountId && x.Status == ProjectStatus.Done).SumAsync(x => x.Price);
        var recentProjects = await _projectService.GetAllProjects(new ProjectFilterModel()
        {
            AccountId = accountId
        });
        var recentProducts = await _deliverableProductService.GetAllDeliverableProduct(
            new DeliverableProductFilterModel()
            {
                AccountId = accountId
            });

        return new StaffDashboardModel()
        {
            NumOfYourOngoingProject = numOfYourOngoingProject,
            NumOfWaitingChecking = numOfWaitingChecking,
            YourTotalPaid = (float)yourTotalPaid,
            RecentProducts = recentProducts,
            RecentProjects = recentProjects
        };
    }
}
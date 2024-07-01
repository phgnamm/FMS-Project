using ChillDe.FMS.Repositories.Entities;
using ChillDe.FMS.Repositories.Enums;
using ChillDe.FMS.Repositories.Interfaces;
using ChillDe.FMS.Repositories.ViewModels.ResponseModels;
using ChillDe.FMS.Services.Interfaces;
using ChillDe.FMS.Services.Models.DashboardModels;
using ChillDe.FMS.Services.Models.DeliverableProductModels;
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

    public DashboardService(IUnitOfWork unitOfWork, IClaimsService claimsService, UserManager<Account> userManager,
        IProjectService projectService, IDeliverableProductService deliverableProductService)
    {
        _unitOfWork = unitOfWork;
        _claimsService = claimsService;
        _userManager = userManager;
        _projectService = projectService;
        _deliverableProductService = deliverableProductService;
    }

    public async Task<ResponseDataModel<AdministratorDashboardModel>> GetAdminDashboard()
    {
        var userId = _claimsService.GetCurrentUserId;
        var numOfFreelancer = await _unitOfWork.DbContext.Freelancer.CountAsync();
        var numOfAccount = await _userManager.Users.CountAsync();
        var totalPaid = await _unitOfWork.DbContext.Project.Where(x => x.Status == ProjectStatus.Done)
            .SumAsync(x => x.Price);
        var numberOfProject = await _unitOfWork.DbContext.Project.CountAsync();
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
            Message = "Get admin dashboard successfully",
            Data = commonDashboard
        };
    }

    private async Task<StaffDashboardModel> GetCommonDashboard(Guid accountId)
    {
        var numOfYourOngoingProject = await _unitOfWork.DbContext.Project
            .Where(x => x.Status != ProjectStatus.Done && x.Status != ProjectStatus.Closed).CountAsync();
        var numOfWaitingChecking = await _unitOfWork.DbContext.DeliverableProduct.Include(x => x.ProjectApply)
            .ThenInclude(x => x.Project).Where(x =>
                x.Status == DeliverableProductStatus.Checking && x.ProjectApply.Project.AccountId == accountId)
            .CountAsync();
        var yourTotalPaid = await _unitOfWork.DbContext.Project
            .Where(x => x.AccountId == accountId && x.Status == ProjectStatus.Done).SumAsync(x => x.Price);
        var recentProjects = await _projectService.GetAllProjects(new ProjectFilterModel()
        {
            AccountId = accountId
        });
        var recentProducts = await _deliverableProductService.GetAllDeliverableProduct(new DeliverableProductFilterModel()
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
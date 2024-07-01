using ChillDe.FMS.Repositories.ViewModels.ResponseModels;
using ChillDe.FMS.Services.Models.DashboardModels;

namespace ChillDe.FMS.Services.Interfaces;

public interface IDashboardService
{
    Task<ResponseDataModel<AdministratorDashboardModel>> GetAdminDashboard();
    Task<ResponseDataModel<StaffDashboardModel>> GetStaffDashboard();
}
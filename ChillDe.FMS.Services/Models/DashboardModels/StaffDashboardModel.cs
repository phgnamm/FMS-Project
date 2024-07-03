using ChillDe.FMS.Services.Models.DeliverableProductModels;
using ChillDe.FMS.Services.Models.ProjectModels;

namespace ChillDe.FMS.Services.Models.DashboardModels;

public class StaffDashboardModel
{
    public int NumOfYourOngoingProject { get; set; }
    public int NumOfWaitingChecking { get; set; }
    public float YourTotalPaid { get; set; }
    public List<ProjectModel>? RecentProjects { get; set; }
    public List<DeliverableProductModel>? RecentProducts { get; set; }
}
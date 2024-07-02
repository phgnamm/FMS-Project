using ChillDe.FMS.Services.Models.DeliverableProductModels;
using ChillDe.FMS.Services.Models.ProjectApplyModels;

namespace ChillDe.FMS.Services.Models.DashboardModels;

public class FreelancerDashboardModel
{
    public int Warning { get; set; }
    public float Wallet { get; set; }
    public int RemainTasks { get; set; }
    public List<ProjectApplyModel>? RecentProjects { get; set; }
    public List<DeliverableProductModel>? RecentProducts { get; set; }
}
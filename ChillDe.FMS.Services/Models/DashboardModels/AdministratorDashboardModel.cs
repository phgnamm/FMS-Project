namespace ChillDe.FMS.Services.Models.DashboardModels;

public class AdministratorDashboardModel : StaffDashboardModel
{
    public int NumOfProject { get; set; }
    public int NumOfFreelancer { get; set; }
    public int NumOfAccount { get; set; }
    public float TotalPaid { get; set; }
}
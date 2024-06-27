using ChillDe.FMS.Repositories.Enums;

namespace ChillDe.FMS.Services.Models.DeliverableProductModels;

public class DeliverableProductUpdateModel
{
    public DeliverableProductStatus status { get; set; }
    public string? feedback { get; set; }
}
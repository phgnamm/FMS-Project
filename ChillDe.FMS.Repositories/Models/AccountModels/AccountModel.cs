using ChillDe.FMS.Repositories.Entities;
using ChillDe.FMS.Repositories.Enums;

namespace ChillDe.FMS.Repositories.Models.AccountModels;

public class AccountModel : BaseEntity
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public Gender Gender { get; set; }
    public DateTime DateOfBirth { get; set; }
    public string? Address { get; set; }
    public string? Image { get; set; }
    public string? Code { get; set; }
    public string Role { get; set; }
    public Guid RoleId { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
    public DateTime CreationDate { get; set; }
}
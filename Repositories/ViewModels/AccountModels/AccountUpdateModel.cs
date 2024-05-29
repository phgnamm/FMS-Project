using System.ComponentModel.DataAnnotations;
using Repositories.Enums;

namespace Repositories.ViewModels.AccountModels;

public class AccountUpdateModel
{
    [Required(ErrorMessage = "FirstName is required")]
    [StringLength(50, ErrorMessage = "FirstName must be no more than 50 characters")]
    public string FirstName { get; set; }

    [Required(ErrorMessage = "LastName is required")]
    [StringLength(50, ErrorMessage = "LastName must be no more than 50 characters")]
    public string LastName { get; set; }

    [Required(ErrorMessage = "Gender is required")]
    [EnumDataType(typeof(Gender), ErrorMessage = "Invalid Gender")]
    public Gender Gender { get; set; }

    [Required(ErrorMessage = "Date of Birth is required")]
    public DateTime DateOfBirth { get; set; }

    [Required(ErrorMessage = "PhoneNumber is required"), Phone(ErrorMessage = "Invalid phone format")]
    [StringLength(15, ErrorMessage = "PhoneNumber must be no more than 15 characters")]
    public string PhoneNumber { get; set; }

    [Required(ErrorMessage = "Email is required"), EmailAddress(ErrorMessage = "Invalid email format")]
    [StringLength(256, ErrorMessage = "Email must be no more than 256 characters")]
    public string Email { get; set; }

    [Required(ErrorMessage = "Code is required")]
    [StringLength(50, ErrorMessage = "Code must be no more than 50 characters")]
    public string Code { get; set; }

    [Required(ErrorMessage = "Role is required")]
    [EnumDataType(typeof(Role), ErrorMessage = "Invalid Role")]
    public Role Role { get; set; }
}
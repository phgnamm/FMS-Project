using Microsoft.AspNetCore.Identity;

namespace ChillDe.FMS.Repositories.Entities
{
	public class Role : IdentityRole<Guid>
	{
		public string? Description { get; set; }
	}
}

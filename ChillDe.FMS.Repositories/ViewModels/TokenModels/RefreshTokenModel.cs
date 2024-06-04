using System.ComponentModel.DataAnnotations;

namespace ChillDe.FMS.Repositories.ViewModels.TokenModels
{
	public class RefreshTokenModel
	{
		[Required(ErrorMessage = "Access Token is required")]
		public string AccessToken { get; set; }
		public string? RefreshToken { get; set; }
	}
}

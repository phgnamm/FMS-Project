using System.ComponentModel.DataAnnotations;

namespace ChillDe.FMS.Repositories.ViewModels.CommonModels
{
	public class LoginGoogleIdTokenModel
	{
		[Required(ErrorMessage = "Id Token is required")]
		public string IdToken { get; set; }
	}
}

using System.ComponentModel.DataAnnotations;

namespace HuskyRescue.ViewModel.Account
{
	public class ExternalLoginConfirmationViewModel
	{
		[Required]
		[Display(Name = "Email")]
		public string Email { get; set; }
	}
}

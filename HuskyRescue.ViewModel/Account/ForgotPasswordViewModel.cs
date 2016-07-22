using System.ComponentModel.DataAnnotations;

namespace HuskyRescue.ViewModel.Account
{
	public class ForgotPasswordViewModel
	{
		[Required]
		[EmailAddress]
		[Display(Name = "Email")]
		public string Email { get; set; }
	}
}

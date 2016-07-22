using System.ComponentModel.DataAnnotations;

namespace HuskyRescue.ViewModel.AccountManager
{
	public class AddPhoneNumberViewModel
	{
		[Required]
		[Phone]
		[Display(Name = "Phone Number")]
		public string Number { get; set; }
	}
}

using System.Collections.Generic;
using System.ComponentModel;

namespace HuskyRescue.ViewModel.Admin.Users
{
	public class List
	{
		public string Id { get; set; }

		[DisplayName("Email Address")]
		public string Email { get; set; }

		[DisplayName("Is Email Confirmed?")]
		public bool EmailConfirmed { get; set; }

		[DisplayName("Phone Number")]
		public string PhoneNumber { get; set; }

		[DisplayName("Is Phone Number Confirmed?")]
		public bool PhoneNumberConfirmed { get; set; }

		[DisplayName("Is 2 Factor Authentication Enabled?")]
		public bool TwoFactorEnabled { get; set; }

		[DisplayName("User locked out since")]
		public string LockoutEndDateUtc { get; set; }

		[DisplayName("Is User Locked Out")]
		public bool LockoutEnabled { get; set; }

		[DisplayName("Number of Failed Login Attempts")]
		public int AccessFailedCount { get; set; }

		[DisplayName("User Name")]
		public string UserName { get; set; }

		public string Roles { get; set; }

		public List()
		{
		}
	}
}

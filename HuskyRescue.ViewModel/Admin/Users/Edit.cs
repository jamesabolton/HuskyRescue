using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using ExpressiveAnnotations.Attributes;

namespace HuskyRescue.ViewModel.Admin.Users
{
	public class Edit
	{
		public string Id { get; set; }
		
		[DisplayName("Email Address")]
		[Required]
		[AssertThat("IsEmail(Email) && Length(Email) <= 256", ErrorMessage = "valid email address required")]
		public string Email { get; set; }

		[DisplayName("Is Email Confirmed?")]
		public bool EmailConfirmed { get; set; }

		[DisplayName("Phone Number")]
		[AssertThat(@"IsRegexMatch(PhoneNumber, '^\\d+$')", ErrorMessage = "cell must be a valid phone number")]
		[AssertThat("Length(PhoneNumber) > 8 && Length(PhoneNumber) <= 20", ErrorMessage = "phone number must be between 9 and 20 digits")]
		public string PhoneNumber { get; set; }

		[DisplayName("Is Phone Number Confirmed?")]
		public bool PhoneNumberConfirmed { get; set; }

		[DisplayName("Is 2 Factor Authentication Enabled?")]
		public bool TwoFactorEnabled { get; set; }

		[DisplayName("User locked out since")]
		public DateTime? LockoutEndDateUtc { get; set; }

		[DisplayName("Is User Locked Out")]
		public bool LockoutEnabled { get; set; }

		[DisplayName("Number of Failed Login Attempts")]
		public int AccessFailedCount { get; set; }

		[DisplayName("User Name")]
		[Required]
		[AssertThat("Length(UserName) <= 256", ErrorMessage = "user name must be less than 256 characters long")]
		public string UserName { get; set; }

		public List<Role> Roles { get; set; }

		public Edit()
		{
			Roles = new List<Role>();
		}
	}
}

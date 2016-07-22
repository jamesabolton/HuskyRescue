using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using ExpressiveAnnotations.Attributes;

namespace HuskyRescue.ViewModel.Admin.Users
{
	public class Create
	{
		public Create()
		{
			Roles = new List<Role>();
		}

		[DisplayName("Email Address")]
		[Required]
		[AssertThat("IsEmail(Email) && Length(Email) <= 256", ErrorMessage = "valid email address required")]
		public string Email { get; set; }

		[DisplayName("Phone Number")]
		[AssertThat(@"IsRegexMatch(PhoneNumber, '^\\d+$')", ErrorMessage = "cell must be a valid phone number")]
		[AssertThat("Length(PhoneNumber) > 8 && Length(PhoneNumber) <= 20", ErrorMessage = "phone number must be between 9 and 20 digits")]
		public string PhoneNumber { get; set; }

		[DisplayName("Is 2 Factor Authentication Enabled?")]
		public bool TwoFactorEnabled { get; set; }

		[DisplayName("User Name")]
		[Required]
		[AssertThat("Length(UserName) <= 256", ErrorMessage = "user name must be less than 256 characters long")]
		public string UserName { get; set; }

		public List<Role> Roles { get; set; }
	}
}
